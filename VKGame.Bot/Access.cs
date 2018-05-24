using System;
using System.Collections.Generic;
using System.Text;

namespace VKGame.Bot
{

    /*=======================================
     * Права доступа пользователя
     * 
     * User (1) - Обычный пользователь
     * Tester(2) - Доступны тестовые команды
     * Moderator(3) - Доступны команды управления ботом
     * Admin(4) - Полностью все комады. 
     =======================================*/

    public enum Access
    {
        User =1,
        Tester =2,
        Moderator=3,
        Admin =4
    }
}
