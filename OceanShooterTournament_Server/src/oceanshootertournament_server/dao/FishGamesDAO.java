/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.dao;

import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import oceanshootertournament_server.model.Room;
import oceanshootertournament_server.thread.GamePlayThread;

/**
 *
 * @author ADMIN
 */
public class FishGamesDAO {
    private static Map<String, GamePlayThread> dictFishGames = new ConcurrentHashMap<>();
    public static synchronized void AddDictFishGame(String roomID, GamePlayThread fishGame) {
        dictFishGames.put(roomID, fishGame);
    }

    public static synchronized void RemoveDictFishGames(String roomID) {
        dictFishGames.remove(roomID);
    }

    public static GamePlayThread GetFishGames(String roomID) {
        return dictFishGames.get(roomID);
    }
}
