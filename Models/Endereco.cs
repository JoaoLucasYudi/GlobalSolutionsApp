﻿// ViaCepConsumerApp/Models/Endereco.cs
using System.Text.Json.Serialization;

namespace ViaCepConsumerApp.Models
{
    public class Endereco
    {
        [JsonPropertyName("cep")]
        public string? Cep { get; set; } 

        [JsonPropertyName("logradouro")]
        public string? Logradouro { get; set; }

        [JsonPropertyName("complemento")]
        public string? Complemento { get; set; }

        [JsonPropertyName("unidade")]
        public string? Unidade { get; set; }

        [JsonPropertyName("bairro")]
        public string? Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string? Localidade { get; set; }

        [JsonPropertyName("uf")]
        public string? Uf { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("regiao")]
        public string? Regiao { get; set; }

        [JsonPropertyName("ibge")]
        public string? Ibge { get; set; }

        [JsonPropertyName("gia")]
        public string? Gia { get; set; }

        [JsonPropertyName("ddd")]
        public string? Ddd { get; set; }

        [JsonPropertyName("siafi")]
        public string? Siafi { get; set; }

        // Propriedade para indicar erro retornado pela API ViaCEP
        [JsonPropertyName("erro")]
        public bool Erro { get; set; } = false;
    }
}