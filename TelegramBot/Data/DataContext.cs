﻿using JobFinder.Models;
using JobFinder.TelegramState;
using JobFinder.TelegramState.StateMachine;
using MongoDB.Driver;

namespace JobFinder.Data;

public class DataContext
{
    private MongoClient _client;
    private IMongoDatabase _context;

    public DataContext()
    {
    
        _client = new MongoClient("mongodb://localhost:27017");
        
        _context = _client.GetDatabase("JobFinder");

        Settings = _context.GetCollection<UserSettings>("Settings");

        GetSettings = _context.GetCollection<SettingState>("Settings");
        
        Vacancies = _context.GetCollection<Vacancies>("Vacancies");
        
        StateMachine = _context.GetCollection<State>("TelegramState");
        
        SettingState = _context.GetCollection<SettingState>("SettingState");

        SearchState = _context.GetCollection<SearchState>("SearchState");
    }


    public IMongoCollection<SettingState> SettingState { get; set; }
    public IMongoCollection<SearchState> SearchState { get; set; }
    
    public IMongoCollection<SettingState> GetSettings { get; set; }
    public IMongoCollection<UserSettings> Settings { get; set; }
    public IMongoCollection<Vacancies> Vacancies { get; set; }
    public IMongoCollection<State> StateMachine { get; set; }

}