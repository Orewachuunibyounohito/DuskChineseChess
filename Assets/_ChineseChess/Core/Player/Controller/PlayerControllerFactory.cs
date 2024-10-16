using System;
using ChineseChess.Controller;

namespace ChineseChess
{
    public class PlayerControllerFactory
    {
        public static IController Generate(PlayerControlType controlType, Player player){
            return controlType switch{
                PlayerControlType.HumanControl => new HumanController(player),
                _ => throw new Exception("Unrecognized ControlType.")
            };
        }
    }
}
