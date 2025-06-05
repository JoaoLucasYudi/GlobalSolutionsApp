// ViaCepConsumerApp/Services/FarolService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViaCepConsumerApp.Models; // Para Farol, StatusFarol, Endereco

namespace ViaCepConsumerApp.Services
{
    public class FarolService
    {
        private readonly List<Farol> _farois;
        private readonly ViaCepService _viaCepService; // Dependência do ViaCepService

        public FarolService(ViaCepService viaCepService) // Construtor para injeção de dependência
        {
            _farois = new List<Farol>();
            _viaCepService = viaCepService ?? throw new ArgumentNullException(nameof(viaCepService));
        }

        public async Task<bool> AdicionarFarolAsync(string nome, StatusFarol status, int nivelEnergia, string cep)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Erro: O nome do farol não pode ser vazio.");
                return false;
            }

            if (_farois.Any(f => f.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"Erro: Já existe um farol com o nome '{nome}'.");
                return false;
            }

            if (nivelEnergia < 0 || nivelEnergia > 100)
            {
                Console.WriteLine("Erro: Nível de energia deve ser entre 0 e 100.");
                return false;
            }

            var novoFarol = new Farol(nome, status, nivelEnergia, cep);

            if (!string.IsNullOrWhiteSpace(cep))
            {
                // Tenta obter o endereço completo ao adicionar
                novoFarol.EnderecoCompleto = await _viaCepService.GetEnderecoAsync(cep);
                if (novoFarol.EnderecoCompleto == null || novoFarol.EnderecoCompleto.Erro)
                {
                    Console.WriteLine($"Aviso: Não foi possível obter os detalhes do endereço para o CEP {cep}, ou o CEP é inválido/não encontrado.");
                }
            }

            _farois.Add(novoFarol);
            Console.WriteLine($"Farol '{nome}' adicionado com sucesso!");
            return true;
        }

        public List<Farol> ListarFarois()
        {
            if (!_farois.Any())
            {
                // Não imprime nada aqui, deixa a UI lidar com lista vazia se quiser
            }
            return _farois; // Retorna a lista, mesmo que vazia
        }

        public bool RemoverFarol(string nomeOuId)
        {
            if (string.IsNullOrWhiteSpace(nomeOuId))
            {
                Console.WriteLine("Nome ou ID do farol para remoção não pode ser vazio.");
                return false;
            }

            Farol? farolParaRemover = _farois.FirstOrDefault(f =>
                f.Nome.Equals(nomeOuId, StringComparison.OrdinalIgnoreCase) ||
                f.Id.ToString().Equals(nomeOuId, StringComparison.OrdinalIgnoreCase));

            if (farolParaRemover != null)
            {
                _farois.Remove(farolParaRemover);
                Console.WriteLine($"Farol '{farolParaRemover.Nome}' removido com sucesso.");
                return true;
            }
            else
            {
                Console.WriteLine($"Farol com nome ou ID '{nomeOuId}' não encontrado.");
                return false;
            }
        }

        public Farol? BuscarFarolPorNomeOuId(string nomeOuId)
        {
            if (string.IsNullOrWhiteSpace(nomeOuId)) return null;
            return _farois.FirstOrDefault(f =>
                f.Nome.Equals(nomeOuId, StringComparison.OrdinalIgnoreCase) ||
                f.Id.ToString().Equals(nomeOuId, StringComparison.OrdinalIgnoreCase));
        }
    }
}