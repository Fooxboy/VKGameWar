using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VKGame.Bot.Commands
{
    public class Competitions : ICommand
    {
        public string Name => "Соревнования";
        public string Arguments => "(), (Вариант_выбора)";
        public string Caption => "Здесь можно узнать о новых соревнованиях и поучаствовать в них!";
        public TypeResponse Type => TypeResponse.Text;
        public List<string> Commands => new List<string>() {"список","бой", "участвовать", "создать" };
        public Access Access => Access.User;


        public object Execute(Models.Message msg)
        {
            var messageArray = msg.body.Split(' ');
            if (messageArray.Length == 1)
                return GetCompetitionsText(msg);
            
            var type = typeof(Competitions);
            var result = Helpers.Command.CheckMethods(type, messageArray[1], msg);
            if (result != null) return result;
            var word = Common.SimilarWord(messageArray[1], Commands);
            return $"❌ Неизвестная подкоманда." +
                   $"\n ❓ Возможно, Вы имели в виду - {Name} {word}";
        }

        public static void EndCompetition(long id)
        {
            //конец.
            try
            {
                var competition = new Api.Competitions(id);
                var listCompetitions = Api.Competitions.AllList;
                var priceWinners = (competition.Price / 2) / 3;
                var priceUsers = (competition.Price / 2) / competition.Members.Count;
                var topUsers = competition.Top;
                var members = competition.Members;
                foreach (var user in topUsers)
                {
                    Api.Message.Send($"❤ Соревнование закончилсь! ТЫ ВОШЁЛ В ТОП 3! ВАШ ВЫИГРЫШ: {priceWinners}", user.Id);
                    Notifications.EnterPaymentCard(Convert.ToInt32(priceWinners), user.Id, "конец соревнования");
                }

                foreach (var user in members)
                {
                    Api.Message.Send($"❤ Соревнование закончилсь! Вы получили: {priceUsers}", user.Id);
                    Notifications.EnterPaymentCard(Convert.ToInt32(priceUsers), user.Id, "конец соревнования");
                }
            }catch(Exception e)
            {
                Statistics.NewError();
                Logger.WriteError(e);
            }
        }

        [Attributes.Trigger("список")]
        public static string List(Models.Message msg)
        {
            string competitionsStr = String.Empty;
            var listCompetitions = Api.Competitions.AllList;
            if (listCompetitions.Count == 0)
                competitionsStr = "🤗 Новых соревнований пока что нет. Зайдите сюда позже :)";
            foreach(var idCompetitions in listCompetitions)
            {
                string topUsers = String.Empty;
                var competition = new Api.Competitions(idCompetitions);
                

                foreach (var top in competition.Top)
                {
                    topUsers += $"😲 {top.Top}-- [id{top.Id}|{top.Name}]\n ⚔ Побед: {top.WinBattles}\n";
                }
                if (competition.Top == null || competition.Top.Count == 0) topUsers = "🤗 Лучших игроков пока что нет.";
        
                competitionsStr += $"➡ Название: {competition.Name}" +
                                 $"\n🆔 ID: {competition.Id}" +
                                 $"\n💰 Фонд: {competition.Price} " +
                                 $"\n🕒 До конца осталось {competition.Time} часов." +
                                 $"\n🔝 Лучшие игроки:" +
                                 $"{topUsers}" +
                                 $"\n \n";
                
            }
            return competitionsStr;
        }

        public static void EndBattle(long userWin, long userLose, long compId, long battleId)
        {
            var competition = new Api.Competitions(compId);
            Models.CompetitionsList.Member member = null;
            var members = competition.Members;
            foreach (var memb in members)
            {
                if(memb.Id == userWin)
                {
                    member = memb;
                    break;
                }
            }
            
            if(member != null)
            {
                members.Remove(member);
                member.WinBattles += 1;
                members.Add(member);

                Api.Message.Send($"✅ Вам засчитана победа в соревнованиях! Теперь у Вас {member.WinBattles} побед.", userWin);
                //Происходит выставление топа.
                var sorted = members.OrderByDescending(u => u.WinBattles);
                int i = 0;
                var listTopMembers = new List<Models.CompetitionsList.TopMember>();
                foreach(var memberTop in sorted)
                {
                    if (i > 3 || i == 3) break;
                    var memberTopModel = new Models.CompetitionsList.TopMember
                    {
                        Id = memberTop.Id,
                        Name = memberTop.Name,
                        Top = ++i,
                        WinBattles = memberTop.WinBattles
                    };
                    listTopMembers.Add(memberTopModel);
                }
                competition.Top = listTopMembers;
            }

        }

        [Attributes.Trigger("бой")]
        public static string Battle(Models.Message msg)
        {
            var resources = new Api.Resources(msg.from_id);
            var user = new Api.User(msg.from_id);
            if (user.Competition == 0) return "❌ Вы не участвуете в соревновании. Для того, чтобы участвовать, напишите: Соревнование участвовать";
            var competition = new Api.Competitions(user.Competition);

            long battleId = 0;

            Statistics.BattleCompetition();
            if (competition.FreeBattle ==0)
            {
                var registry = new Api.Registry(user.Id);
                ++registry.CountCreateBattles;
                if (user.BattleId != 0) return "❌ Вы уже находитесь в другой битве.";
                long userHp = 0;
                var price = 0;
                var builds = new Api.Builds(user.Id);
                userHp = Bot.Commands.Battle.API.HpUser(user.Id);
                battleId = Api.Battles.Create($"соревнование {competition.Name}", user.Id, price, userHp);
                user.BattleId = battleId;
                competition.FreeBattle = battleId;
              //  Api.Registry.SetRegistry(registry);
                return "‼ Вы создали битву! Подождите пока кто-либо вступит!";

            }
            else
            {
               var battle = new Api.Battles(competition.FreeBattle);
               var builds = new Api.Builds(user.Id);
               if (user.BattleId != 0) return "❌ Вы уже находитесь в другой битве.";

               user.BattleId = battle.Id;
               var userHp = Bot.Commands.Battle.API.HpUser(user.Id);
              //  battle.UserTwo = user.Id;
              //  battle.HpTwo = userHp;
              //  battle.IsStart = true;
                 competition.FreeBattle = 0;
                Api.Message.Send("‼ К Вам в битву вступили! Вы атакуете первый! Атаковать так же как и в обычных битвах.", battle.Creator);
                return "‼ Вы вступили в битву! Сейчас ход Вашего врага!";
            }
        }

        [Attributes.Trigger("участвовать")]
        public static string Join(Models.Message msg)
        {
            var resources = new Api.Resources(msg.from_id);
            if (resources.TicketsCompetitions == 0) return "❌ У тебя нет билетов!";
            resources.TicketsCompetitions = resources.TicketsCompetitions - 1;
            var messageArray = msg.body.Split(' ');
            long idComp = 0;
            try
            {
                idComp = Int64.Parse(messageArray[2]);
            }
            catch (IndexOutOfRangeException)
            {
                return "❌ Вы не указали ID соревнования!";
            }catch(FormatException)
            {
                return "❌ Вы указали неверный ID соревнования!";
            }

            if (!Api.Competitions.Check(idComp)) return "❌ Такого соревнования не существует!";
            var competition = new Api.Competitions(idComp);
            if (competition.IsEnd) return "❌ Это соревнование уже закончилось!";
            var members = competition.Members;
            var user = new Api.User(msg.from_id);
            if(user.Competition != 0) return "❌ Вы уже учавствуете в соревновании!";
            var member = new Models.CompetitionsList.Member();
            member.Id = user.Id;
            member.Name = user.Name;
            member.WinBattles = 0;
            members.Add(member);
            competition.Members = members;
            user.Competition = competition.Id;
            Statistics.JoinCompetition();
            return "✅ Вы успешно участвуете в соревновании. Для начала атаки напишите: соревнования бой";
        }

        [Attributes.Trigger("создать")]
        public static string Create(Models.Message msg)
        {
            var user = new Api.User(msg.from_id);
            if (user.Access < 4) return "❌ Вам недоступна подкоманда.";
            var messageArray = msg.body.Split(' ');
            string name = messageArray[2];
            long price = Int64.Parse(messageArray[3]);
            long time = Int64.Parse(messageArray[4]);
            var idComp = Api.Competitions.New(name, price, time);
            var listComp = Api.Competitions.AllList;
            listComp.Add(idComp);
            Statistics.NewCompetition();
            //TODO: Создание соревнований не работает как положено. Доделать.
            return $"Вы создали соревнование с ID - {idComp}";
        }

        private string GetCompetitionsText(Models.Message msg)
        {
            return "❓ КРАТКАЯ ПОМОЩЬ:" +
                   "\n➡ Соревнования список - выводит текущий список соревнований." +
                   "\n➡ Соревнования участвовать ID - вы вступаете в соревнование с айди - ID. Узнать ID - команда выше." +
                   "\n➡ Соревнования бой - вы вступаете в бой. Если нет свободного боя, создается новый. Принцип боя такой же как и в обычной. ";
        }
    }
}