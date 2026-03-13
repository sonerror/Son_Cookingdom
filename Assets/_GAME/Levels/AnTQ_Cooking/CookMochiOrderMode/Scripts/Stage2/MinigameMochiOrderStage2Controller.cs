using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace sonnv
{
    public class MinigameMochiOrderStage2Controller : MonoBehaviour
    {
        [SerializeField] private CatMochiOrderStage2Controller catMochiOrderStage2Controller;
        [SerializeField] private RabbitMochiOrderStage2Controller rabbitMochiOrderStage2Controller;
        [SerializeField] private FlourMochiOrderStage2Controller flourMochiOrderStage2Controller;

        public CatMochiOrderStage2Controller GetCatMochiOrderStage2Controller()
        {
            return catMochiOrderStage2Controller;
        }

        public RabbitMochiOrderStage2Controller GetRabbitMochiOrderStage2Controller()
        {
            return rabbitMochiOrderStage2Controller;
        }

        public FlourMochiOrderStage2Controller GetFlourMochiOrderStage2Controller()
        {
            return flourMochiOrderStage2Controller;
        }

        public void EventRabbitKneadFlourCorrect()
        {
            catMochiOrderStage2Controller.EventRabbitKneadFlourCorrect();
        }

        public void EventRabbitKneadFlourWrong()
        {
            catMochiOrderStage2Controller.EventRabbitKneadFlourWrong();

            _ = ResetAllCharacter();
        }

        public async Task ResetAllCharacter()
        {
            await Task.Delay(3000);
            catMochiOrderStage2Controller.ResetCharacter();
            rabbitMochiOrderStage2Controller.ResetCharacter();
            flourMochiOrderStage2Controller.ResetCharacter();
        }

    }
}

