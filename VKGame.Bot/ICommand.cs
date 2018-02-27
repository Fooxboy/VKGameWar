
namespace VKGame.Bot
{
    public interface ICommand
    {
        string Name { get; }
        string Arguments { get; }
        string Caption { get; }
        TypeResponse Type { get; }
        object Execute(LongPollVK.Models.AddNewMsg msg);
    }

    public enum TypeResponse
    {
        Text, Photo, TextAndPhoto, Console
    }
}