namespace Portfolio.Content;

public enum Lang { Pt, En }

public sealed record Text(string Pt, string En)
{
    public string Of(Lang lang) => lang == Lang.Pt ? Pt : En;
}

public sealed record Project(string Name, string Url, Text Summary, string[] Stack);

public static partial class Site
{
    public const string Name = "Yuri Augusto";
    public const string Handle = "yuriafp";
    public const string Email = "yuri13_yuri@hotmail.com";
    public const string GitHubUrl = "https://github.com/yuriafp";
    public const string LinkedInUrl = "https://www.linkedin.com/in/yurifpaiva/";

    public const string AboutId = "sobre";
    public const string ProjectsId = "projetos";
    public const string ContactId = "contato";

    public static readonly Text Tagline = new(
        "Desenvolvedor .NET",
        ".NET developer");

    public static readonly Text About = new(
        "Trabalho com C# e .NET, principalmente em APIs e serviços de back-end.",
        "I work with C# and .NET, mostly on back-end APIs and services.");

    public static readonly Text Education = new(
        "Bacharel em Ciência da Computação — Universidade de Franca",
        "BSc in Computer Science — Universidade de Franca");

    public static readonly Text StackNowLabel = new("Trabalho com", "Working with");
    public static readonly Text StackLearningLabel = new("Estudando", "Learning");

    public static readonly string[] StackNow =
    [
        "C#", ".NET", "ASP.NET Core", "EF Core", "Blazor",
        "SQL Server", "PostgreSQL", "RabbitMQ", "Docker", "xUnit",
    ];

    public static readonly Text[] StackLearning =
    [
        new("System design", "System design"),
        new("Microsserviços", "Microservices"),
        new("Redis", "Redis"),
        new("OAuth2 / JWT", "OAuth2 / JWT"),
        new("NoSQL", "NoSQL"),
        new("CI/CD", "CI/CD"),
    ];

    public static readonly Text NavAbout = new("Sobre", "About");
    public static readonly Text NavProjects = new("Projetos", "Projects");
    public static readonly Text NavContact = new("Contato", "Contact");

    public static readonly Text ContactLead = new(
        "Aberto a oportunidades e a conversar sobre qualquer um destes projetos.",
        "Open to opportunities, and happy to talk about any of these projects.");

    public static readonly Text BuiltWith = new(
        "Feito em Blazor WebAssembly. Roda inteiro no seu navegador.",
        "Built with Blazor WebAssembly. Runs entirely in your browser.");

    public static readonly Text NotFound = new(
        "Não há nada neste endereço.",
        "There's nothing at this address.");
    public static readonly Text BackHome = new("Voltar ao início", "Back to home");
}
