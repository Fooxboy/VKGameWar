using System;
using JetBrains.Annotations;
using VkNet.Model.RequestParams;
using System.Collections.Generic;
using VKGame.Bot.Api;

namespace VKGame.Bot.Commands
{
    /// <summary>
    /// Класс для работы с вводимыми данными.
    /// </summary>
    public class WriteData:ICommand
    {
        public override string Name => "!";
        public override string Caption => "Используется для ввода данных. Например, при смени имени.";
        public override string Arguments => "(данные)";
        public override TypeResponse Type => TypeResponse.Text;
        public override List<string> Commands => new List<string>();
        public override Access Access => Access.User;
        public override string HelpUrl => "сслыка недоступна";

        public override object Execute(Models.Message msg)
        {
            string text = "";
            string[] arrayText = msg.body.Split(' ');
            for (int i = 1; arrayText.Length > i; i++) text += $"{arrayText[i]} ";
            
            try
            {
                var command = Common.LastCommand[msg.from_id];

                //получение бустеров пользователей
                var boosters = new Api.Boosters(msg.from_id);          
                
                //проверка на команду в кэше.
                if(command  is Start) return Start.SetNick(msg, text);
                else if (command is Army)
                {
                    //Я понятия не имею что тут работает и даже не знаю работает ли....
                    int count = 0;
                    int type = 0;
                    try
                    {
                        if (Common.CountCreateArmySoldiery[msg.from_id] != 0)
                        {
                            count = Common.CountCreateArmySoldiery[msg.from_id];
                            type = 1;
                            Common.CountCreateArmySoldiery.Remove(msg.from_id);
                            boosters.CreateSoldiery -= 1;
                        }
                        else if (Common.CountCreateArmyTanks[msg.from_id] != 0)
                        {
                            count = Common.CountCreateArmyTanks[msg.from_id];
                            type = 2;
                            Common.CountCreateArmyTanks.Remove(msg.from_id);
                            boosters.CreateTanks -= 1;
                        }
                        else count = 0;
                    }
                    catch (KeyNotFoundException)
                    {
                        count = 0;
                    }

                    var value = text;

                    if (value.ToLower() == "да")
                    {
                        if (type == 1)
                        {
                            Bot.Commands.Army.Api.CreateSoldiery(count, msg.from_id, true);
                        }else if (type == 2)
                        {
                            Bot.Commands.Army.Api.CreateTanks(count, msg.from_id, true);
                        }

                        return "✅ Вы успешно создаёте армию с усилителем!";
                    }else if (value.ToLower() == "нет")
                    {
                        if (type == 1)
                        {
                            Bot.Commands.Army.Api.CreateSoldiery(count, msg.from_id, false);
                        }else if (type == 2)
                        {
                            Bot.Commands.Army.Api.CreateTanks(count, msg.from_id, false);
                        }
                        
                        return "✅ Вы успешно создаёте армию без усилителя!";

                    }else if (value.ToLower() == "умолчание")
                    {
                        var config = new Api.ConfigBoosters(msg.from_id);
                        if (type == 1)
                        {
                            Bot.Commands.Army.Api.CreateSoldiery(count, msg.from_id, Convert.ToBoolean(config.CreateSoldiery));
                        }else if (type == 2)
                        {
                            Bot.Commands.Army.Api.CreateTanks(count, msg.from_id, Convert.ToBoolean(config.CreateTanks));
                        }
                        var registry = new Api.Registry(msg.from_id);
                        registry.ShowNotifyBoostArmy = false;
                        return "✅ Вы успешно создаёте армию со значением по умолчанию";
                    }
                    else
                    {
                        var config = new Api.ConfigBoosters(msg.from_id);
                        if (type == 1)
                        {
                            Bot.Commands.Army.Api.CreateSoldiery(count, msg.from_id, Convert.ToBoolean(config.CreateSoldiery));
                        }else if (type == 2)
                        {
                            Bot.Commands.Army.Api.CreateTanks(count, msg.from_id, Convert.ToBoolean(config.CreateTanks));
                        }

                        return $"✅ Не было распознано значение. Было использовано значение по умолчанию - {Convert.ToBoolean(config.CreateSoldiery)}";
                    }
                }
                else
                {
                    return "1❌" + " Не найдена команда, для которой нужны данные.";
                }            
            }
            catch (KeyNotFoundException)
            {
                return "2❌" + " Не найдена команда, для которой нужны данные.";

            }
        }
    }
}