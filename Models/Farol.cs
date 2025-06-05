// ViaCepConsumerApp/Models/Farol.cs
using System;
using System.ComponentModel;

// Certifique-se que Endereco.cs está no mesmo namespace ou que há um 'using' se estiver diferente.
// Como ambos estarão em ViaCepConsumerApp.Models, não precisa de 'using' extra aqui para Endereco.

namespace ViaCepConsumerApp.Models
{
    public enum StatusFarol
    {
        [Description("Online")]
        Online,
        [Description("Offline")]
        Offline,
        [Description("Em Manutenção")]
        Manutencao,
        [Description("Desconhecido")]
        Desconhecido
    }

    public class Farol
    {
        public Guid Id { get; private set; }
        public string Nome { get; set; }
        public StatusFarol Status { get; set; }
        public int NivelEnergia { get; set; }
        public string Cep { get; set; }
        public Endereco? EnderecoCompleto { get; set; } // Referencia a classe Endereco

        public Farol(string nome, StatusFarol status, int nivelEnergia, string cep)
        {
            Id = Guid.NewGuid();
            Nome = nome; // Assumindo que nome não será nulo pela lógica de UI
            Status = status;
            NivelEnergia = nivelEnergia;
            Cep = cep; // Assumindo que cep não será nulo pela lógica de UI
        }

        public override string ToString()
        {
            string enderecoStr = "Não informado ou CEP inválido";
            if (EnderecoCompleto != null && !EnderecoCompleto.Erro && !string.IsNullOrWhiteSpace(EnderecoCompleto.Logradouro))
            {
                enderecoStr = $"{EnderecoCompleto.Logradouro}, {EnderecoCompleto.Bairro} - {EnderecoCompleto.Localidade}/{EnderecoCompleto.Uf}";
            }
            else if (!string.IsNullOrWhiteSpace(Cep))
            {
                enderecoStr = $"CEP: {Cep} (Detalhes do endereço não disponíveis)";
                if (EnderecoCompleto != null && EnderecoCompleto.Erro)
                {
                    enderecoStr = $"CEP: {Cep} (CEP não encontrado ou inválido na API)";
                }
            }

            return $"ID: {Id}\n" +
                   $"Nome: {Nome}\n" +
                   $"Status: {GetEnumDescription(Status)}\n" +
                   $"Nível de Energia: {NivelEnergia}%\n" +
                   $"Localização (CEP: {Cep}): {enderecoStr}\n" +
                   "-----------------------------------";
        }

        public static string GetEnumDescription(StatusFarol value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return value.ToString(); // Salvaguarda
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}