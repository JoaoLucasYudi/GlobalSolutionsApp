// ViaCepConsumerApp/Program.cs
using System;
using System.Threading.Tasks;
using ViaCepConsumerApp.Models;
using ViaCepConsumerApp.Services;

namespace ViaCepConsumerApp
{
    class Program
    {
        // Instâncias das classes Services
        private static readonly ViaCepService _viaCepService = new ViaCepService();
        private static readonly FarolService _farolService = new FarolService(_viaCepService);
        private static readonly AuthService _authService = new AuthService();

        private static User? _currentUser = null;


        // Início da lógica do aplicativo
        static async Task Main(string[] args)
        {
            // Função de login do usuário obrigatória, chama as funções de login ou de sair do aplicativo
            bool appRunning = true;
            while (appRunning)
            {
                if (_currentUser == null)
                {
                    Console.Clear();
                    Console.WriteLine("--- Sistema de Gerenciamento de Faróis ---");
                    Console.WriteLine("Você precisa estar logado para continuar.");
                    Console.WriteLine("\n1. Login");
                    Console.WriteLine("0. Sair do Aplicativo");
                    Console.Write("Opção: ");
                    string? choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            LoginUI();
                            break;
                        case "0":
                            appRunning = false;
                            Console.WriteLine("Saindo do aplicativo...");
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Pressione Enter para continuar.");
                            Console.ReadLine();
                            break;
                    }
                }
                else // Usuário está logado
                {
                    await ShowMainMenuAsync(appIsRunning: () => appRunning, setAppRunning: (val) => appRunning = val);
                }
            }
        }

        // função de login
        static void LoginUI()
        {
            Console.Clear();
            Console.WriteLine("--- Login ---");
            Console.Write("Nome de Usuário: ");
            string? username = Console.ReadLine();
            Console.Write("Senha: ");
            string? password = ReadPassword();

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                _currentUser = _authService.LoginUser(username, password);
            }
            else
            {
                Console.WriteLine("Nome de usuário e senha não podem ser vazios.");
            }

            if (_currentUser == null)
            {
                Console.WriteLine("Falha no login. Pressione Enter para continuar.");
                Console.ReadLine();
            }
        }

        static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }

        // Controle do menu principal e das funcionalidades do aplicativo
        // Os nomes das classes estão bem claras e representam suas funcionalidades
        // A ideia é simular uma API CRUD para gerenciar nossa solução
        // 1º funcionalidade relevante: Adicionar Farol
        // 2º funcionalidade relevante: Listar Faróis
        // 3º funcionalidade relevante: Ver Detalhes de um Farol Específico
        // 4º funcionalidade relevante: Atualizar Farol
        // 5º funcionalidade relevante: Remover Farol

        static async Task ShowMainMenuAsync(Func<bool> appIsRunning, Action<bool> setAppRunning)
        {
            Console.Clear();
            Console.WriteLine($"--- Menu Principal (Logado como: {_currentUser?.Username}) ---");
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1. Adicionar Novo Farol");  
            Console.WriteLine("2. Listar Todos os Faróis");
            Console.WriteLine("3. Ver Detalhes de um Farol Específico");
            Console.WriteLine("4. Atualizar Farol");
            Console.WriteLine("5. Remover Farol"); 
            Console.WriteLine("0. Sair do Aplicativo"); 
            Console.Write("Opção: ");

            string? escolha = Console.ReadLine();

            switch (escolha)
            {
                case "1":
                    await AdicionarFarolUIAsync();
                    break;
                case "2":
                    ListarFaroisUI();
                    break;
                case "3":
                    await VerDetalhesFarolUIAsync();
                    break;
                case "4":
                    await AtualizarFarolUIAsync();
                    break;
                case "5":
                    RemoverFarolUI();
                    break;
                case "0": 
                    setAppRunning(false);
                    Console.WriteLine("Saindo do aplicativo...");
                    return; 
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            // Se o app ainda está rodando (não escolheu sair)
            if (appIsRunning())
            {
                Console.WriteLine("\nPressione Enter para voltar ao menu principal...");
                Console.ReadLine();
            }
        }

        // 1º funcionalidade relevante: Adicionar Farol
        // O farol contém os seguintes atributos: Nome, Status, Nível de Energia e CEP (opcional)
        // O modelo do Farol já está definido na classe Farol
        // Uso de Enum para Status do Farol
        // O CEP é fornecido pela conexão com a API ViaCEP
        static async Task AdicionarFarolUIAsync()
        {
            Console.Clear();
            Console.WriteLine("\n--- Adicionar Novo Farol ---");
            Console.Write("Nome do Farol: ");
            string nome = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Nome não pode ser vazio.");
                return;
            }

            StatusFarol status = StatusFarol.Desconhecido;
            bool statusValido = false;
            while (!statusValido)
            {
                Console.WriteLine("Status do Farol:");
                int i = 0;
                foreach (StatusFarol sValue in Enum.GetValues(typeof(StatusFarol)))
                {
                    Console.WriteLine($"{i}. {Farol.GetEnumDescription(sValue)}");
                    i++;
                }
                Console.Write("Escolha o número do status: ");
                if (int.TryParse(Console.ReadLine(), out int statusIndex) && Enum.IsDefined(typeof(StatusFarol), statusIndex))
                {
                    status = (StatusFarol)statusIndex;
                    statusValido = true;
                }
                else
                {
                    Console.WriteLine("Seleção de status inválida. Tente novamente.");
                }
            }

            int nivelEnergia = -1;
            while (nivelEnergia < 0 || nivelEnergia > 100)
            {
                Console.Write("Nível de Energia (0-100%): ");
                if (!int.TryParse(Console.ReadLine(), out nivelEnergia) || nivelEnergia < 0 || nivelEnergia > 100)
                {
                    Console.WriteLine("Nível de energia inválido. Deve ser um número entre 0 e 100.");
                    nivelEnergia = -1;
                }
            }

            Console.Write("CEP (8 dígitos, ex: 01001000, deixe em branco se não houver): ");
            string cep = Console.ReadLine() ?? "";

            await _farolService.AdicionarFarolAsync(nome, status, nivelEnergia, cep);
        }

        // 2º funcionalidade relevante: Listar Faróis
        // Lista todos os faróis cadastrados e se não houver faróis cadastrados retorna uma mensagem informando que não há faróis cadastrados
        static void ListarFaroisUI()
        {
            Console.Clear();
            Console.WriteLine("\n--- Lista de Faróis ---");
            var farois = _farolService.ListarFarois();
            if (!farois.Any())
            {
                Console.WriteLine("Nenhum farol cadastrado.");
                return;
            }
            foreach (var farol in farois)
            {
                Console.WriteLine(farol.ToString());
            }
        }

        // 3º funcionalidade relevante: Ver Detalhes de um Farol Específico
        // Permite ao usuário buscar um farol pelo nome
        // Fornece todas as informações do farol
        static async Task VerDetalhesFarolUIAsync()
        {
            Console.Clear();
            Console.WriteLine("\n--- Ver Detalhes do Farol ---");
            Console.Write("Digite o Nome ou ID do farol: ");
            string nomeOuId = Console.ReadLine() ?? "";
            var farol = _farolService.BuscarFarolPorNomeOuId(nomeOuId);

            if (farol != null)
            {
                Console.WriteLine(farol.ToString());
                if ((farol.EnderecoCompleto == null || farol.EnderecoCompleto.Erro) && !string.IsNullOrWhiteSpace(farol.Cep))
                {
                    Console.WriteLine("Tentando obter/atualizar detalhes do endereço via CEP...");
                    farol.EnderecoCompleto = await _viaCepService.GetEnderecoAsync(farol.Cep);
                    if (farol.EnderecoCompleto != null && !farol.EnderecoCompleto.Erro)
                    {
                        Console.WriteLine("Detalhes do endereço atualizados:");
                        Console.WriteLine(farol.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível obter os detalhes do endereço para o CEP informado ou o CEP é inválido.");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Farol '{nomeOuId}' não encontrado.");
            }
        }

        // 4º funcionalidade relevante: Atualizar Farol
        // Permite ao usuário atualizar as informações de um farol existente
        static async Task AtualizarFarolUIAsync()
        {
            Console.Clear();
            Console.WriteLine("\n--- Atualizar Farol ---");
            Console.Write("Digite o Nome ou ID do farol para atualizar: ");
            string nomeOuId = Console.ReadLine() ?? "";
            Farol? farolParaAtualizar = _farolService.BuscarFarolPorNomeOuId(nomeOuId);

            if (farolParaAtualizar == null)
            {
                Console.WriteLine($"Farol '{nomeOuId}' não encontrado.");
                return;
            }

            Console.WriteLine($"Farol selecionado: {farolParaAtualizar.Nome} (ID: {farolParaAtualizar.Id})");

            Console.Write($"Novo Nome (atual: {farolParaAtualizar.Nome}, deixe em branco para não alterar): ");
            string novoNome = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(novoNome) && !_farolService.ListarFarois().Any(f => f.Nome.Equals(novoNome, StringComparison.OrdinalIgnoreCase) && f.Id != farolParaAtualizar.Id))
            {
                farolParaAtualizar.Nome = novoNome;
            }
            else if (!string.IsNullOrWhiteSpace(novoNome))
            {
                Console.WriteLine("Nome já existe ou é inválido. Nome não alterado.");
            }

            Console.WriteLine($"Status atual: {Farol.GetEnumDescription(farolParaAtualizar.Status)}");
            Console.WriteLine("Novo Status do Farol (deixe em branco para não alterar):");
            int i = 0;
            foreach (StatusFarol sValue in Enum.GetValues(typeof(StatusFarol)))
            {
                Console.WriteLine($"{i}. {Farol.GetEnumDescription(sValue)}");
                i++;
            }
            Console.Write("Escolha o número do novo status ou deixe em branco: ");
            string inputStatus = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(inputStatus) && int.TryParse(inputStatus, out int statusIndex) && Enum.IsDefined(typeof(StatusFarol), statusIndex))
            {
                farolParaAtualizar.Status = (StatusFarol)statusIndex;
            }
            else if (!string.IsNullOrWhiteSpace(inputStatus))
            {
                Console.WriteLine("Seleção de status inválida. Status não alterado.");
            }

            Console.Write($"Novo Nível de Energia (atual: {farolParaAtualizar.NivelEnergia}%, deixe em branco para não alterar): ");
            string inputEnergia = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(inputEnergia))
            {
                if (int.TryParse(inputEnergia, out int novoNivelEnergia) && novoNivelEnergia >= 0 && novoNivelEnergia <= 100)
                {
                    farolParaAtualizar.NivelEnergia = novoNivelEnergia;
                }
                else
                {
                    Console.WriteLine("Nível de energia inválido. Nível não alterado.");
                }
            }

            Console.Write($"Novo CEP (atual: {farolParaAtualizar.Cep}, deixe em branco para não alterar): ");
            string novoCep = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(novoCep) && novoCep != farolParaAtualizar.Cep)
            {
                farolParaAtualizar.Cep = novoCep;
                Console.WriteLine("Atualizando endereço com base no novo CEP...");
                farolParaAtualizar.EnderecoCompleto = await _viaCepService.GetEnderecoAsync(novoCep);
                if (farolParaAtualizar.EnderecoCompleto == null || farolParaAtualizar.EnderecoCompleto.Erro)
                {
                    Console.WriteLine($"Aviso: Não foi possível obter os detalhes do endereço para o novo CEP {novoCep}, ou o CEP é inválido/não encontrado.");
                }
            }

            Console.WriteLine($"Farol '{farolParaAtualizar.Nome}' atualizado.");
            Console.WriteLine(farolParaAtualizar.ToString());
        }

        // 5º funcionalidade relevante: Remover Farol
        // Permite ao usuário remover um farol pelo nome cadastrado
        static void RemoverFarolUI()
        {
            Console.Clear();
            Console.WriteLine("\n--- Remover Farol ---");
            Console.Write("Digite o Nome ou ID do farol a ser removido: ");
            string nomeOuId = Console.ReadLine() ?? "";
            _farolService.RemoverFarol(nomeOuId);
        }
    }
}