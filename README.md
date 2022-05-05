# IAV Proyecto Final - Dungeon Crawler: The Controller
 
## Propuesta
 
Se propone para este proyecto la realización de un **Dungeon Crawler** (al estilo Enter the Gungeon) **controlado de manera automática**, e implementando distintos agentes inteligentes.
 
Para ello, se dividirá el trabajo en varias secciones:
 
- **Creación aleatoria con memoria del mapa** (*Daniel Illanes Morillas*): Se creará un mapa de juego estilo roguelike de manera pseudo-aleatoria, siguiendo una serie de reglas predefinidas básicas. Además, la “dificultad” del mapa se ajustará al nivel del jugador, equilibrando las recompensas y castigos en función del desempeño del jugador.
 
- **Enemigos, Jefe Final y "Game Master"** (*Aarón Nauzet Moreno Sosa*): Habrá distintos agentes inteligentes en el mapa, cada uno de ellos con su máquina de estados, que intentarán entorpecer al jugador. Se destaca el jefe final, con un árbol de comportamiento; y el "game master", un agente que intentará entorpecer al jugador.
 
- **Controlador automático del jugador** (*Sergio José Alfonso Rojas*): Habrá un agente inteligente que tratará de pasarse el juego de forma automática. Llevará esto a cabo teniendo en cuenta la situación del mapa (zonas inexploradas, distancia a zonas útiles o peligrosas, etc), el equipamiento (comenzará con un arma sencilla y podrá encontrar armas mejores en cofres del escenario) y parámetros como la vida, la armadura o la munición actual (buscará consumibles que recompongan estos parámetros antes de afrontar batallas difíciles). Asimismo, contará con un set de movimientos equivalente al juego de referencia, destacando éste por utilizar un una evasión que evita el daño de contacto de los proyectiles enemigos.