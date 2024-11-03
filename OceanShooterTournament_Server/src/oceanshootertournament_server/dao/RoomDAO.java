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
import oceanshootertournament_server.model.Room;

/**
 *
 * @author ADMIN
 */
public class RoomDAO {

    private static Map<String, Room> dictRooms = new ConcurrentHashMap<>();
    private static Queue<String> roomObjectPooling = new LinkedList<>();
    private static int idRoom = 0;

    public static synchronized void AddDictRoom(Room room) {
        String roomID = "";
        System.out.println("SIZE1: " + roomObjectPooling.size());
        if (roomObjectPooling.size() > 0) {
            roomID = roomObjectPooling.poll();
        } else {
            idRoom++;
            roomID = "room" + idRoom;
        }
        room.setIdRoom(roomID);
        dictRooms.put(roomID, room);
    }

    public static synchronized void RemoveDictRoom(String roomID) {
        dictRooms.remove(roomID);
        roomObjectPooling.add(roomID);
        System.out.println("SIZE2: " + roomObjectPooling.size());
    }

    public static Room GetRoom(String roomID) {
        return dictRooms.get(roomID);
    }

    public static List<Room> GetListRoomCurrent() {
        List<Room> tmp = new ArrayList<>();
        for (Map.Entry<String, Room> entry : dictRooms.entrySet()) {
            tmp.add(entry.getValue());
        }
        return tmp;
    }

    public static synchronized boolean CheckHaveRoom(String roomID) {
        return dictRooms.containsKey(roomID);
    }
}
