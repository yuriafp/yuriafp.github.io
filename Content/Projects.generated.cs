// Gerado por tools/sync-projects.ps1 a partir do .portfolio.json de cada
// repositorio. Nao edite a mao: a proxima sincronizacao sobrescreve.

namespace Portfolio.Content;

public static partial class Site
{
    public static readonly Project[] Projects =
    [
        new("Shortly",
            "https://github.com/yuriafp/Shortly",
            new("API REST de encurtamento de URL, feita como projeto acadêmico. Padrão Result para erros de domínio, repositórios separados por leitura e escrita, middleware global de exceção e suíte de testes em xUnit.",
                "REST API for URL shortening, built as an academic project. Result pattern for domain errors, read and write repositories kept separate, a global exception middleware, and an xUnit test suite."),
            ["ASP.NET Core", "PostgreSQL", "EF Core", "AutoMapper", "xUnit"]),

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

        new("TicTacToe",
            "https://github.com/yuriafp/TicTacToe",
            new("Jogo da velha em Blazor WebAssembly, publicado como site estático no GitHub Pages. A CPU tem três níveis: aleatório, heurístico e jogo perfeito por minimax com poda alfa-beta, verificado por busca exaustiva como invencível.",
                "Tic-tac-toe in Blazor WebAssembly, published as a static site on GitHub Pages. The CPU has three levels: random, heuristic, and perfect play through minimax with alpha-beta pruning, verified unbeatable by exhaustive search."),
            ["Blazor WASM", ".NET 8", "GitHub Pages"]),
    ];
}
