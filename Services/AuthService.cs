// ViaCepConsumerApp/Services/AuthService.cs
using System;
using ViaCepConsumerApp.Models;

namespace ViaCepConsumerApp.Services
{
    public class AuthService
    {
        // Credenciais fixas
        private const string HardcodedUsername = "admin";
        private const string HardcodedPassword = "0000";


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