// Shooting engine cells:
var weapon = cells[0][0];
var typeToDestroy = 3;

weapon.power(1);
for(var i = 0; i < radar.getWidth(); i++)
    for(var j = 0; j < radar.getHeight(); j++)
        if(radar.getCell(j, i).type == typeToDestroy)
            weapon.shoot(j, i);
//-------------------------------------------

// Looking for enemy:
var engine = cells[1][4];

engine.power(1);
if(radar.type != 1)
    engine.jump();
//-------------------------------------------

// Repairing damaged cells:
var repairCell = cells[1][2];

repairCell.power(1);
for(var i = 0; i < 3; i++)
    for(var j = 0; j < 7; j++)
        if(cells[i][j].health < 10)
        {
            repairCell.repair(j, i);
        }
//-------------------------------------------

// D3str0y3r 1337:
var engine = cells[1][3];
var weapon1 = cells[0][0];
var weapon2 = cells[0][5];

engine.power(1);
weapon1.power(4);
weapon2.power(4);

if(radar.type != 1)
    engine.jump();

for(var i = 0; i < radar.getWidth(); i++)
    for(var j = 0; j < radar.getHeight(); j++)
        if(radar.getCell(j, i).health > 0)
        {
            weapon1.shoot(j, i);
            weapon2.shoot(j, i);
        }
//-------------------------------------------