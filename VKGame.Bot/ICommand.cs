using System.Collections.Generic;

namespace VKGame.Bot
{
    public interface ICommand
    {
        string Name { get; }
        string Arguments { get; }
        string Caption { get; }
        TypeResponse Type { get; }
        object Execute(Models.Message msg);
        List<string> Commands { get; }
        Access Access { get; }
    }

    public enum TypeResponse
    {
        Text, Photo, TextAndPhoto, Console
    }
}