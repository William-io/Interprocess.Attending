// Cria o builder da aplicação web com os argumentos passados
var builder = WebApplication.CreateBuilder(args);

// Constrói a aplicação web com todas as configurações
var app = builder.Build();

// Configura o pipeline de requisições HTTP
if (!app.Environment.IsDevelopment())
{
    // Adiciona middleware para tratamento de exceções
    app.UseExceptionHandler("/Error");
    // Adiciona cabeçalhos de segurança HSTS (HTTP Strict Transport Security)
    app.UseHsts();
}

// Força redirecionamento para HTTPS
app.UseHttpsRedirection();

// Habilita servir arquivos estáticos (HTML, CSS, JS, imagens)
app.UseStaticFiles();

// Habilita o roteamento de requisições
app.UseRouting();

// Mapeia a rota raiz "/" para redirecionar para o arquivo index.html
app.MapGet("/", () => Results.Redirect("/index.html"));

app.Run();
