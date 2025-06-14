﻿// ViaCepConsumerApp/Models/User.cs
namespace ViaCepConsumerApp.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; } 

        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
        }
    }
}