using UnityEngine;
using UnityEngine.UI;

namespace IAV22_MorenoSosa
{
    /* 
     * La salud es un componente común que se asocia a cualquier objeto que pueda sufrir daños y ser destruido (instalaciones, unidades, pueblos, torres...).
     * Se notificará su 'muerte' a los objetos interesados cuando su salud llegue a 0.
     * 
     * Posibles mejoras:
     * - Tener una forma de reiniciar al valor original de salud
     */
    public class Health : MonoBehaviour
    {
        // La cantidad inicial de salud con la que comenzar
        [SerializeField] private int _initialAmmount = 1; // Valor por defecto
        public int InitialAmmount { get { return _initialAmmount; } }

        private Slider slider;

        // Evento 'en caso de muerte' que protege el delegado muerte, al que los objetos se suscriben
        // Realmente no estoy usando esta suscripción para nada...
        public delegate void Death();
        public event Death OnDeath;

        // La cantidad de salud actual
        public int Amount { get; private set; }

        /*********************************************************/

        // Despierta el componente e inicializa la cantidad actual de salud
        private void Awake()
        {
            Amount = InitialAmmount;
            slider = GetComponentInChildren<Slider>();

            if (slider != null)
            {
                slider.maxValue = InitialAmmount;
                slider.value = InitialAmmount;
            }
        }

        // La salud recibe cierta cantida de daño
        public void TakeDamage(int amount)
        {
            Amount -= amount;

            // No permite que la salud baje de cero
            if (Amount <= 0) {
                Amount = 0;
                // Dispara un evento cuando el objeto asociado ha muerto, y quita dicha referencia
                if (OnDeath != null) {
                    OnDeath();
                    OnDeath = null;
                }
            }

            if (slider != null)
                slider.value = Amount;
        }
    }
}