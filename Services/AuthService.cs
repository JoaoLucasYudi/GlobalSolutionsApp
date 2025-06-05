// ViaCepConsumerApp/Services/AuthService.cs
using System;
// System.Collections.Generic e System.Linq não são mais estritamente necessários aqui
using ViaCepConsumerApp.Models;

namespace ViaCepConsumerApp.Services
{
    public class AuthService
    {
        // Credenciais fixas
        private const string HardcodedUsername = "admin";
        private const string HardcodedPassword = "0000";

        // O método RegisterUser foi removido.
        // A lista _users foi removida.

        public User? LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Nome de usuário e senha são obrigatórios para login.");
                return null;
            }

            // Valida contra o usuário e senha fixos
            if (username.Equals(HardcodedUsername, StringComparison.OrdinalIgnoreCase) &&
                password == HardcodedPassword)
            {
                Console.WriteLine($"Login bem-sucedido! Bem-vindo, {HardcodedUsername}.");
                // Retorna uma nova instância de User para o usuário admin
                // A senha aqui é apenas para preencher o modelo User, não é usada para nova validação.
                return new User(HardcodedUsername, HardcodedPassword);
            }
            else
            {
                Console.WriteLine("Nome de usuário ou senha inválidos.");
                return null;
            }
        }
    }
}