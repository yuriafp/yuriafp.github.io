// Gerado por tools/sync-projects.ps1 a partir do .portfolio.json de cada
// repositorio. Nao edite a mao: a proxima sincronizacao sobrescreve.

namespace Portfolio.Content;

public static partial class Site
{
    public static readonly Project[] Projects =
    [
        new("Oclock",
            "https://github.com/yuriafp/Oclock",
            new("API de controle de jornada de funcionários. O registro de ponto é assíncrono: a API publica numa fila e responde 202 Accepted, e um worker consome e persiste. A infraestrutura de desenvolvimento sobe em container.",
                "Employee time-tracking API. Clocking in is asynchronous: the API publishes to a queue and returns 202 Accepted, while a worker consumes the message and persists it. Development infrastructure runs in containers."),
            [".NET 8", "ASP.NET Core", "RabbitMQ", "SQL Server", "Docker"]),

        new("Shortly",
            "https://github.com/yuriafp/Shortly",
            new("API REST de encurtamento de URL, feita como projeto acadêmico. Padrão Result para erros de domínio, repositórios separados por leitura e escrita, middleware global de exceção e suíte de testes em xUnit.",
                "REST API for URL shortening, built as an academic project. Result pattern for domain errors, read and write repositories kept separate, a global exception middleware, and an xUnit test suite."),
            ["ASP.NET Core", "PostgreSQL", "EF Core", "AutoMapper", "xUnit"]),

        new("VagasBauru",
            "https://github.com/yuriafp/VagasBauru",
            new("Portal de vagas de emprego de Bauru. Blazor WebAssembly no cliente, API em ASP.NET Core no servidor e uma biblioteca compartilhada com os contratos entre os dois. Persistência com EF Core.",
                "Job board for Bauru. Blazor WebAssembly on the client, an ASP.NET Core API on the server, and a shared library holding the contracts between them. Persistence with EF Core."),
            ["Blazor WASM", "ASP.NET Core", "EF Core"]),

        new("BinaryCupCake",
            "https://github.com/yuriafp/BinaryCupCake",
            new("Loja de cupcakes em Blazor WebAssembly: cadastro de usuários e produtos, autenticação e configuração de permissões. Projeto Integrador Transdisciplinar em Ciência da Computação.",
                "Cupcake shop in Blazor WebAssembly: user and product registration, authentication, and permission setup. Built as a computer science capstone project."),
            ["Blazor WASM", "PostgreSQL", "Azure"]),

        new("FileOrganizer",
            "https://github.com/yuriafp/FileOrganizer",
            new("Serviço em background que vigia uma pasta — a de Downloads, por exemplo — e arquiva por extensão o que chega nela. Leve e configurável, roda como Windows Service.",
                "Background worker service that watches a folder — your Downloads directory, for instance — and files whatever lands there by extension. Lightweight and configurable, runs as a Windows Service."),
            [".NET Worker Service", "Windows Service"]),
    ];
}
