using System;

namespace BoosterINI
{
    public class SmpRate : Program
    {
        /// <summary>
        /// Вопрос, меняем ли SmpRate у SV
        /// </summary>
        public void EditSmpRate()
        {
            try
            {
                Console.Write("Меняем smpRate у всех устройств? [ДА - y, НЕТ - любой символ]: ");
                var userAnswer = Convert.ToString(Console.ReadLine());
                if (userAnswer == "y")
                {
                    // Запоминаем новое значение smpRate
                    smpRateEdit = true;
                    Console.Write("Новое значение smpRate = ");
                    smpRateValue = Convert.ToInt32(Console.ReadLine());
                }
                else
                {
                    smpRateEdit = false;
                }

                Console.Write("\n");
            }
            catch (Exception)
            {
                ErrorMessage = "Ошибка при чтении вашего ответа";
            }
        }
    }
}