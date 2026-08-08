<#
    Lê o .portfolio.json da raiz de cada repositório público e regrava
    Content/Projects.generated.cs. O workflow roda isto uma vez por dia.

    Para conferir uma descrição antes de commitar no repo do projeto:
        pwsh tools/sync-projects.ps1 -LocalManifests tools/manifests
#>
param(
    [string]$User = 'yuriafp',
    [string]$Output = 'Content/Projects.generated.cs',
    [string]$LocalManifests
)

$ErrorActionPreference = 'Stop'

function ConvertTo-CSharpLiteral([string]$value) {
    $escaped = $value.Replace('\', '\\').Replace('"', '\"')
    $escaped = $escaped -replace "`r", '' -replace "`n", '\n'
    return '"' + $escaped + '"'
}

function Test-Manifest($manifest, [string]$name) {
    if (-not $manifest.summary -or
        -not $manifest.summary.pt -or
        -not $manifest.summary.en -or
        -not $manifest.stack -or
        @($manifest.stack).Count -eq 0) {

        throw "O .portfolio.json de $name e invalido. Campos obrigatorios: summary.pt, summary.en e stack (array nao vazio)."
    }
}

$headers = @{ 'User-Agent' = 'yuriafp-portfolio'; 'Accept' = 'application/vnd.github+json' }
if ($env:GITHUB_TOKEN) {
    $headers['Authorization'] = "Bearer $env:GITHUB_TOKEN"
} else {
    Write-Warning 'Sem GITHUB_TOKEN: o limite anonimo da API e de 60 requisicoes por hora.'
}

$repos = Invoke-RestMethod -Uri "https://api.github.com/users/$User/repos?per_page=100&type=owner" -Headers $headers
$owned = @($repos | Where-Object { -not $_.fork })

Write-Host "$($owned.Count) repositorio(s) publico(s) de autoria propria."

$projects = @()
$skipped = @()

foreach ($repo in $owned) {
    $manifest = $null

    if ($LocalManifests) {
        $path = Join-Path $LocalManifests "$($repo.name).portfolio.json"
        if (Test-Path $path) {
            $manifest = Get-Content $path -Raw -Encoding UTF8 | ConvertFrom-Json
        }
    } else {
        $rawHeaders = $headers.Clone()
        $rawHeaders['Accept'] = 'application/vnd.github.raw'
        $uri = "https://api.github.com/repos/$User/$($repo.name)/contents/.portfolio.json"
        try {
            # -UseBasicParsing e obrigatorio no Windows PowerShell 5.1: sem ele a
            # resposta passa pelo motor do Internet Explorer e falha fora do CI.
            $response = Invoke-WebRequest -Uri $uri -Headers $rawHeaders -UseBasicParsing
            $manifest = [System.Text.Encoding]::UTF8.GetString($response.RawContentStream.ToArray()) | ConvertFrom-Json
        } catch {
            $status = 0
            if ($_.Exception.Response) { $status = [int]$_.Exception.Response.StatusCode }

            # So 404 significa repositorio sem manifesto. Limite de taxa, rede ou
            # JSON malformado precisam aparecer, nao virar "nao tem manifesto".
            if ($status -ne 404) {
                throw "Falha ao ler .portfolio.json de $($repo.name) (HTTP $status): $($_.Exception.Message)"
            }

            $manifest = $null
        }
    }

    if (-not $manifest) {
        $skipped += $repo.name
        continue
    }

    # Manifesto quebrado aborta o build. Repositorio sumindo do site em silencio
    # e pior que a publicacao falhar de forma visivel.
    Test-Manifest $manifest $repo.name

    $order = 999
    if ($null -ne $manifest.order) { $order = [int]$manifest.order }

    $projects += [pscustomobject]@{
        Name    = $repo.name
        Url     = "https://github.com/$User/$($repo.name)"
        Pt      = [string]$manifest.summary.pt
        En      = [string]$manifest.summary.en
        Stack   = @($manifest.stack)
        Order   = $order
    }
}

if ($projects.Count -eq 0) {
    throw 'Nenhum repositorio com .portfolio.json. Abortando para nao publicar a pagina sem projetos.'
}

$sorted = $projects | Sort-Object Order, Name

$lines = New-Object System.Collections.Generic.List[string]
$lines.Add('// Gerado por tools/sync-projects.ps1 a partir do .portfolio.json de cada')
$lines.Add('// repositorio. Nao edite a mao: a proxima sincronizacao sobrescreve.')
$lines.Add('')
$lines.Add('namespace Portfolio.Content;')
$lines.Add('')
$lines.Add('public static partial class Site')
$lines.Add('{')
$lines.Add('    public static readonly Project[] Projects =')
$lines.Add('    [')

$first = $true
foreach ($project in $sorted) {
    if (-not $first) { $lines.Add('') }
    $first = $false

    $stack = ($project.Stack | ForEach-Object { ConvertTo-CSharpLiteral ([string]$_) }) -join ', '

    $lines.Add('        new(' + (ConvertTo-CSharpLiteral $project.Name) + ',')
    $lines.Add('            ' + (ConvertTo-CSharpLiteral $project.Url) + ',')
    $lines.Add('            new(' + (ConvertTo-CSharpLiteral $project.Pt) + ',')
    $lines.Add('                ' + (ConvertTo-CSharpLiteral $project.En) + '),')
    $lines.Add('            [' + $stack + ']),')
}

$lines.Add('    ];')
$lines.Add('}')

Set-Content -Path $Output -Value $lines -Encoding UTF8

Write-Host "$($projects.Count) projeto(s) gravados em $Output."
if ($skipped.Count -gt 0) {
    Write-Host "Sem .portfolio.json, ficaram de fora: $($skipped -join ', ')"
}
