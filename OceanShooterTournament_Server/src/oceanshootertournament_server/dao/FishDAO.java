/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.dao;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Queue;
import java.util.concurrent.ConcurrentHashMap;
import oceanshootertournament_server.model.Fish;

/**
 *
 * @author pc
 */
public class FishDAO {
    private Map<String, Fish> dictFishs;
    private static Queue<String> fishObjectPooling;
    private static int idFish = 0;

    public FishDAO() {
        dictFishs = new ConcurrentHashMap<>();
        fishObjectPooling = new LinkedList<>();
    }

    public synchronized void AddDictFish(Fish fish) {
        String fishID = "";
        if (fishObjectPooling.size() > 0) {
            fishID = fishObjectPooling.poll();
        } else {
            idFish++;
            fishID = "fish" + idFish;
        }
        fish.setFishID(fishID);
        dictFishs.put(fishID, fish);
    }

    public synchronized void RemoveDictBullet(String fishID) {
        dictFishs.remove(fishID);
        fishObjectPooling.add(fishID);
    }

    public Fish GetBullet(String fishID) {
        return dictFishs.get(fishID);
    }

    public boolean IsEmpty() {
        return dictFishs.isEmpty();
    }

    public List<Fish> listFishs() {
        List<Fish> list = new ArrayList<>();
        for (Map.Entry<String, Fish> entry : dictFishs.entrySet()) {
            Fish fish = entry.getValue();
            list.add(fish);
        }
        return list;
    }
    
    public boolean checkInDict(String fishID) {
        return dictFishs.containsKey(fishID);
    }
}
