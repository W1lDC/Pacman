using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AI4 : MonoBehaviour {

	public Transform target;
	private List<TileManager4.Tile4> tiles = new List<TileManager4.Tile4>();
	private TileManager4 manager;
	public GhostMove4 ghost;
	public TileManager4.Tile4 nextTile = null;
	public TileManager4.Tile4 targetTile;
	TileManager4.Tile4 currentTile;

	void Awake()
	{
		manager = GameObject.Find("Game Manager4").GetComponent<TileManager4>();
		tiles = manager.tiles;

		if(ghost == null)	Debug.Log ("game object ghost not found");
		if(manager == null)	Debug.Log ("game object Game Manager not found");
	}

	public void AILogic()
	{
        // recebe a tile ocupada no momento
		Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
		currentTile = tiles[manager.Index ((int)currentPos.x, (int)currentPos.y)];
		
		targetTile = GetTargetTilePerGhost();
		
        // ir para a proxima tile de acordo com a posiçao
		if(ghost.direction.x > 0)	nextTile = tiles[manager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
		if(ghost.direction.x < 0)	nextTile = tiles[manager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
		if(ghost.direction.y > 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
		if(ghost.direction.y < 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y-1))];
		
		if(nextTile.occupied || currentTile.isIntersection)
		{
            // se encontrar uma parede
			if(nextTile.occupied && !currentTile.isIntersection)
			{
                // se o fantasma se move para a esq ou dir e existe uma parede na proxima tile
				if(ghost.direction.x != 0)
				{
					if(currentTile.down == null)	ghost.direction = Vector3.up;
					else 							ghost.direction = Vector3.down;
				}
                // se o fantasma se move para cima ou baixo e existe uma parede na proxima tile
                else if (ghost.direction.y != 0)
				{
					if(currentTile.left == null)	ghost.direction = Vector3.right; 
					else 							ghost.direction = Vector3.left;
				}
			}
            // se encontrar uma interceçao, calcula a distancia para o alvo de todas as tiles disponiveis e escolhe o mais curto
			if(currentTile.isIntersection)
			{
				float dist1, dist2, dist3, dist4;
				dist1 = dist2 = dist3 = dist4 = 999999f;
				if(currentTile.up != null && !currentTile.up.occupied && !(ghost.direction.y < 0)) 		dist1 = manager.distance(currentTile.up, targetTile);
				if(currentTile.down != null && !currentTile.down.occupied &&  !(ghost.direction.y > 0)) 	dist2 = manager.distance(currentTile.down, targetTile);
				if(currentTile.left != null && !currentTile.left.occupied && !(ghost.direction.x > 0)) 	dist3 = manager.distance(currentTile.left, targetTile);
				if(currentTile.right != null && !currentTile.right.occupied && !(ghost.direction.x < 0))	dist4 = manager.distance(currentTile.right, targetTile);
				float min = Mathf.Min(dist1, dist2, dist3, dist4);
				if(min == dist1) ghost.direction = Vector3.up;
				if(min == dist2) ghost.direction = Vector3.down;
				if(min == dist3) ghost.direction = Vector3.left;
				if(min == dist4) ghost.direction = Vector3.right;
			}
		}
        // se existir uma decisao, designa-se o proximo waypoint para o fantasma
		else
		{
			ghost.direction = ghost.direction;	// update do valor do setter
		}
	}

	public void RunLogic()
	{
        // receber a tile ocupada no momento
		Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
		currentTile = tiles[manager.Index ((int)currentPos.x, (int)currentPos.y)];

        // receber a proxima tile de acordo com a direçao
		if(ghost.direction.x > 0)	nextTile = tiles[manager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
		if(ghost.direction.x < 0)	nextTile = tiles[manager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
		if(ghost.direction.y > 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
		if(ghost.direction.y < 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y-1))];


		if(nextTile.occupied || currentTile.isIntersection)
		{
            // se encontrar uma parede
			if(nextTile.occupied && !currentTile.isIntersection)
			{
                // se o fantasma se move para a esq ou dir e existe uma parede na proxima tile

                if (ghost.direction.x != 0)
				{
					if(currentTile.down == null)	ghost.direction = Vector3.up;
					else 							ghost.direction = Vector3.down;
				}
                // se o fantasma se move para cima ou baixo e existe uma parede na proxima tile
                else if (ghost.direction.y != 0)
				{
					if(currentTile.left == null)	ghost.direction = Vector3.right; 
					else 							ghost.direction = Vector3.left;
				}
			}
			// se encontar uma interceçao, escolhe aleatoriamente (rand)
			if(currentTile.isIntersection)
			{
				List<TileManager4.Tile4> availableTiles = new List<TileManager4.Tile4>();
				TileManager4.Tile4 chosenTile;
				if(currentTile.up != null && !currentTile.up.occupied && !(ghost.direction.y < 0)) 			availableTiles.Add (currentTile.up);
				if(currentTile.down != null && !currentTile.down.occupied &&  !(ghost.direction.y > 0)) 	availableTiles.Add (currentTile.down);	
				if(currentTile.left != null && !currentTile.left.occupied && !(ghost.direction.x > 0)) 		availableTiles.Add (currentTile.left);
				if(currentTile.right != null && !currentTile.right.occupied && !(ghost.direction.x < 0))	availableTiles.Add (currentTile.right);
				int rand = Random.Range(0, availableTiles.Count);
				chosenTile = availableTiles[rand];
				ghost.direction = Vector3.Normalize(new Vector3(chosenTile.x - currentTile.x, chosenTile.y - currentTile.y, 0));
			}
		}
        // se nao houver nenhuma decisao a fazer, designar o proximo waypoint para o fantasma
		else
		{
			ghost.direction = ghost.direction;	// update do valor do setter
		}
	}


	TileManager4.Tile4 GetTargetTilePerGhost()
	{
		Vector3 targetPos;
		TileManager4.Tile4 targetTile;
		Vector3 dir;

        // recebe a posiçao da tile do alvo e arredonda para int para se poder usar a funçao Index()
		switch(name)
		{
		case "blinky":	// alvo = pacman
			targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f);
			targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];
			break;
		case "pinky":	// alvo = pacman + 4* direçao dopacman (4 passos a frente do pacman)
			dir = target.GetComponent<PlayerController4>().getDir();
			targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f) + 4*dir;

            // se o pacman estiver a subir, passa a ser 4 passos acima e à esquerda do pacman
            // subtrai-se 4 do X da coordenada do pacman
			if(dir == Vector3.up)	targetPos -= new Vector3(4, 0, 0);

			targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];
			break;
		case "inky":	// alvo = ambushVector(pacman+2 - blinky) adicionado ao pacman+2
			dir = target.GetComponent<PlayerController4>().getDir();
			Vector3 blinkyPos = GameObject.Find ("blinky").transform.position;
			Vector3 ambushVector = target.position + 2*dir - blinkyPos ;
			targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f) + 2*dir + ambushVector;
			targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];
			break;
		case "clyde": // 
			targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f);
			targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];
			if(manager.distance(targetTile, currentTile) < 9)
				targetTile = tiles[manager.Index (0, 2)];
			break;
		default:
			targetTile = null;
			Debug.Log ("TARGET TILE NOT ASSIGNED");
			break;
		}
		return targetTile;
	}
}