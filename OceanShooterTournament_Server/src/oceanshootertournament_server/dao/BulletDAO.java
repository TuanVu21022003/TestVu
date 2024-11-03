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
import oceanshootertournament_server.model.Bullet;

/**
 *
 * @author pc
 */
public class BulletDAO {

    private Map<String, Bullet> dictBullets;
    private static Queue<String> bulletObjectPooling;
    private static int idBullet = 0;

    public BulletDAO() {
        dictBullets = new ConcurrentHashMap<>();
        bulletObjectPooling = new LinkedList<>();
    }

    public synchronized void AddDictBullet(Bullet bullet) {
        String bulletID = "";
        if (bulletObjectPooling.size() > 0) {
            bulletID = bulletObjectPooling.poll();
        } else {
            idBullet++;
            bulletID = "bullet" + idBullet;
        }
        bullet.setBulletID(bulletID);
        dictBullets.put(bulletID, bullet);
    }

    public synchronized void RemoveDictBullet(String bulletID) {
        dictBullets.remove(bulletID);
        bulletObjectPooling.add(bulletID);
    }

    public Bullet GetBullet(String bulletID) {
        return dictBullets.get(bulletID);
    }

    public boolean IsEmpty() {
        return dictBullets.isEmpty();
    }

    public List<Bullet> listBullets() {
        List<Bullet> list = new ArrayList<>();
        for (Map.Entry<String, Bullet> entry : dictBullets.entrySet()) {
            Bullet bullet = entry.getValue();
            list.add(bullet);
        }
        return list;
    }
    
    public boolean checkInDict(String bulletID) {
        return dictBullets.containsKey(bulletID);
    }
}
