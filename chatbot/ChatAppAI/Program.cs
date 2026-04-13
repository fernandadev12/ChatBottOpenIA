
using System.Runtime.CompilerServices;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;

var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"];
string key = config["OpenAIKey"];

//criando o client
IChatClient chatClient = new OpenAIClient(key).GetChatClient(model).AsIChatClient();

List<ChatMessage> chatHistory =
    [
        new ChatMessage(ChatRole.System, """
            Estou animado pelo primeiro chatbot utilizando IA, por favor escolha opções:

            1. Sua localização e previsão do tempo 
            2. O que gostaria de buscar  na sua localidade
        """)
    ];

while(true)
{
    Console.WriteLine(" mensagem:");
    string userMensagem =  Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userMensagem));

    Console.WriteLine ( "Resposta IA:");
    string response = "";
    await foreach(ChatResponseUpdate item in chatClient.GetStreamingResponseAsync(chatHistory))
    {
        Console.WriteLine(item.Text);
        response += item.Text;
    }
    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
    Console.WriteLine();

}