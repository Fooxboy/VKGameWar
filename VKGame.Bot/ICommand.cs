
namespace VKGame.Bot
{
    public interface ICommand
    {
        string Name { get; }
        string Arguments { get; }
        string Caption { get; }
        TypeResponse Type { get; }
        object Execute(Models.Message msg);
    }

    public enum TypeResponse
    {
        Text, Photo, TextAndPhoto, Console
    }
}