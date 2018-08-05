---


---

<h1 id="strategy-game">Strategy Game</h1>
<p>Hello, This is Strategy Game which is made with Unity The game includess three main parts.</p>
<ul>
<li>Infinite scroll</li>
<li>game map</li>
<li>building information part</li>
</ul>
<p>Game screen and part can be seen picture in below.<br>
<img src="https://picasaweb.google.com/115424717854012243209/6586323083186124737#6586323082438506978" alt="Game Screen" title="GameScreen"></p>
<h2 id="infinite-scroll">Infinite Scroll</h2>
<p>On the infinite scroll blue objects represent “Barracks”, yellow objects represent “PowerPlants”.</p>
<p>You can drag this objects to game map.</p>
<p><img src="https://picasaweb.google.com/115424717854012243209/6586324298619486193#6586324300399214770" alt="Infinite Scroll" title="Infinite Scroll"></p>
<p>This infinite scroll is optimized with Object Pooling.</p>
<h2 id="game-map">Game Map</h2>
<p>Game map is includes grid. Its size is 28x24.<br>
<img src="https://picasaweb.google.com/115424717854012243209/6586325072040672609#6586325069423011762" alt="Game Map" title="Game Map"></p>
<h2 id="information-place">Information Place</h2>
<p>On the right side of game map there is an information place,After you locate buildings to game map, you can see the informations of buildings and create soldier from barrack buildings.</p>
<p>Building can not be located on the other buildings and soldiers. Collided grid cells are shown with “red”.<br>
<img src="https://picasaweb.google.com/115424717854012243209/6586326222447642433#6586326223401674930" alt="Collided Cells" title="Collided Cells"></p>
<p>This picture shows the barrack information and create soldier from this barrack with button.<br>
<img src="https://picasaweb.google.com/115424717854012243209/6586325569535260273#6586325571730580562" alt="Barrack Info" title="Barrack Info"></p>
<p>This picture shows the barrack information and create soldier from this barrack with button.<br>
<img src="https://picasaweb.google.com/115424717854012243209/6586325822900744721#6586325826698896834" alt="Powerplant Info" title="PowerPlant Info"></p>
<h2 id="soldiers">Soldiers</h2>
<p>Soldiers can be created from information place with “Create Soldier” button when any barrack is clicked.</p>
<p>Soldiers’ placement starts down-middle gridcell of barrack and surround the barrack.</p>
<p><img src="https://picasaweb.google.com/115424717854012243209/6586327056870695041#6586327056294267698" alt="Soldiers" title="Soldiers"></p>
<p>To move soldiers, firstly you need to click with left button to select then right click to green cell. Soldier will find the shortest path. For the shortest path search A* algorithm is implemented.</p>

