using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace PlanningPoker;

public class MasterSetting
{
    public long Id {  get; set; }
    public long ChatId { get; set; }
    public string MasterCode { get; set; }
    public bool IsMaster { get; set; } = false;
    public MasterSetting(long id, long chatId, string code) { Id = id; ChatId = chatId; MasterCode = code; }

    /// <summary>
    /// Создание кода для настройки мастера группы
    /// </summary>
    /// <returns></returns>
    public static string GenerateCode()
    {
        var newCode = Random.Shared.Next(1010, 9090).ToString();
        return newCode;
    }

    /// <summary>
    /// Проверка правильности кода
    /// </summary>
    /// <returns></returns>
    public static bool CheckCode(string code, string masterСode) => code == masterСode;



}
