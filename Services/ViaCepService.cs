// ViaCepConsumerApp/Services/ViaCepService.cs
using System;
using System.Linq; 
using System.Net.Http;
using System.Net.Http.Json; 
using System.Threading.Tasks;
using ViaCepConsumerApp.Models;

// Classe de serviço para consultar a API ViaCEP
// Usando HttpClient para realizar requisições HTTP seguindo os padrões solicitados
namespace ViaCepConsumerApp.Services
{
    public class ViaCepService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<Endereco?> GetEnderecoAsync(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                Console.WriteLine("CEP não pode ser vazio.");
                return null; // Retorna nulo se o CEP for inválido
            }

            string cepFormatado = new string(cep.Where(char.IsDigit).ToArray());

            if (cepFormatado.Length != 8)
            {
                Console.WriteLine($"Formato de CEP inválido: {cep}. Deve conter 8 dígitos.");
                // Retorna um objeto Endereco com Erro = true para indicar falha na validação local
                return new Endereco { Cep = cep, Erro = true };
            }

            string apiUrl = $"https://viacep.com.br/ws/{cepFormatado}/json/";

            try
            {
                Console.WriteLine($"Consultando API ViaCEP: {apiUrl}");
                Endereco? endereco = await client.GetFromJsonAsync<Endereco>(apiUrl, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (endereco != null && endereco.Erro) // Verifica a flag "erro" retornada pela API
                {
                    Console.WriteLine($"CEP não encontrado na base do ViaCEP: {cepFormatado}");
                    return endereco; // Retorna o objeto com a flag de erro da API
                }
                return endereco;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição HTTP para o CEP {cepFormatado}: {e.Message}");
                return new Endereco { Cep = cepFormatado, Erro = true }; // Indica erro 
            }
            catch (Exception e) // Captura outras exceções (ex: JsonException)
            {
                Console.WriteLine($"Erro ao processar dados do ViaCEP para {cepFormatado}: {e.Message}");
                return new Endereco { Cep = cepFormatado, Erro = true }; // Indica erro
            }
        }
    }
}