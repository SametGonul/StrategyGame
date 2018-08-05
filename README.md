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
<img src="https://lh3.googleusercontent.com/U7wMU6D5UbD66RwGWyh3yR-KHxwW0VE4Wu0tgS8vG4bD3nk22LggKpSur0VhNugg1hzra4eXx8M8" alt="enter image description here" title="Game Screen"></p>
<h2 id="infinite-scroll">Infinite Scroll</h2>
<p>On the infinite scroll blue objects represent “Barracks”, yellow objects represent “PowerPlants”.</p>
<p>You can drag this objects to game map.</p>
<p><img src="https://lh3.googleusercontent.com/lvQTqjAyY6WMzH1SuP61Ry6e7gv9wQzL23KI0whmWM5ucFPGJMUPKnlGoombqTik38uhU5mekdU2" alt="" title="InfiniteScroll"></p>
<p>This infinite scroll is optimized with Object Pooling.</p>
<h2 id="game-map">Game Map</h2>
<p>Game map is includes grid. Its size is 28x24.<br>
<img src="https://lh3.googleusercontent.com/HPxtaRvKN3gSkKSCun7PqMllbtNF1k2mv5aVOBaOEjBHGcIVbxIJSJ_zkfhdJncYPUWkvwdxRtEj" alt="" title="GameMap"></p>
<h2 id="information-place">Information Place</h2>
<p>On the right side of game map there is an information place,After you locate buildings to game map, you can see the informations of buildings and create soldier from barrack buildings.</p>
<p>Building can not be located on the other buildings and soldiers. Collided grid cells are shown with “red”.<br>
<img src="https://lh3.googleusercontent.com/B80qYhjuo7CXSQHdyPbysQSfKWGjX8Ladh3J_pNalGScXTPff4bFD4hX2c-TeUeifTKlSb9cxjX8" alt="" title="CollidedCells"></p>
<p>This picture shows the barrack information and create soldier from this barrack with button.<br>
<img src="https://lh3.googleusercontent.com/cKWuL_k5bgB5Q9h1CZRyBaV4LduBmPHJMZP3sf-vKQCTLyVCEyyvQb3bigsDNkdNzaoDfMILPIZ3" alt="" title="BarrackInfo"></p>
<p>This picture shows the barrack information and create soldier from this barrack with button.<br>
<img src="https://lh3.googleusercontent.com/fWNtOk7CpMSfvZNh1UaEBqQoCspb88cyDNh6psFbLQ3Hg-Wx5QgbRl-1UmA295_wqPzR3LMWaTrw" alt="enter image description here" title="PowerPlantInfo"></p>
<h2 id="soldiers">Soldiers</h2>
<p>Soldiers can be created from information place with “Create Soldier” button when any barrack is clicked.</p>
<p>Soldiers’ placement starts down-middle gridcell of barrack and surround the barrack.</p>
<p><img src="https://lh3.googleusercontent.com/xW8KoyZz5uAN9UANxj81BugFjWn--Mzub1onS-JihnMQsZ6kseXIhZzZl09sEJvmY_ru_-Y5jhfD" alt="enter image description here" title="Soldiers"></p>
<p>To move soldiers, firstly you need to click with left button to select then right click to green cell. Soldier will find the shortest path. For the shortest path search A* algorithm is implemented.</p>
