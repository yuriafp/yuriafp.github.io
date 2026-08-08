# yuriafp.github.io

Site pessoal em Blazor WebAssembly, publicado no GitHub Pages em
<https://yuriafp.github.io>.

Página única, tema escuro, conteúdo em português e inglês com alternância no
cabeçalho. Roda inteiro no navegador — não há back-end.

## Rodar localmente

```bash
dotnet run
```

## Editar o conteúdo

Os textos fixos — nome, sobre, formação, stack e contato — estão em
[`Content/Site.cs`](Content/Site.cs), em pares `Text(pt, en)`.

Os projetos não ficam aqui. **Cada repositório descreve a si mesmo**, num
`.portfolio.json` na própria raiz:

```json
{
  "order": 1,
  "stack": [".NET 8", "ASP.NET Core", "RabbitMQ"],
  "summary": {
    "pt": "Uma frase ou duas sobre o que o projeto faz.",
    "en": "A sentence or two about what the project does."
  }
}
```

`summary.pt`, `summary.en` e `stack` são obrigatórios. `order` define a posição
no grid e vale 999 se omitido.

Um repositório entra no site quando é público, não é fork e tem esse arquivo.
Repositório privado nem aparece na resposta da API, então não existe o risco de
publicar um card que leve o visitante a um 404.

A pasta [`tools/manifests/`](tools/manifests) guarda cópias dos manifestos dos
projetos atuais — é área de preparo, o que vale é o arquivo na raiz de cada
repositório. Para conferir uma descrição antes de commitar lá:

```bash
pwsh tools/sync-projects.ps1 -LocalManifests tools/manifests
```

## Sincronização

O workflow roda [`tools/sync-projects.ps1`](tools/sync-projects.ps1) uma vez por
dia, às 9h UTC, e regrava `Content/Projects.generated.cs`. Se algo mudou, commita
o arquivo antes de publicar — sem esse commit o próximo push reverteria o site
para as descrições antigas, já que o build usa o arquivo versionado.

Push não sincroniza: publica o que está versionado, sem tocar na API do GitHub.

Manifesto inválido derruba o build de propósito, assim como nenhum projeto ser
encontrado. Repositório sumindo do site em silêncio é pior que a publicação
falhar de forma visível.

## Deploy

Em **Settings → Pages**, a origem precisa estar como **GitHub Actions**.

Dois detalhes que o workflow resolve e que quebram o site se forem removidos:

- **`.nojekyll`** — sem ele o Jekyll ignora a pasta `_framework/`, onde está o
  runtime, e a página fica em branco.
- **`404.html`** — cópia do `index.html`. O Pages devolve 404 para qualquer
  caminho que não seja arquivo real; servir o app nesse 404 deixa o roteador do
  Blazor resolver a rota.

## Tamanho do payload

O primeiro acesso baixa cerca de **2,4 MB** (gzip do CDN do Pages; os `.br` que
o Blazor gera não são usados porque o Pages não negocia `Content-Encoding: br`).
Depois disso fica em cache. `InvariantGlobalization` já corta os ~1,5 MB de dados
do ICU.

Para reduzir mais, instalar a workload e republicar:

```bash
dotnet workload install wasm-tools
```
