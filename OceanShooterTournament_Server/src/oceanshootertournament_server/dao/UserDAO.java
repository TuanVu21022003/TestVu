/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.dao;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;
import oceanshootertournament_server.model.User;

/**
 *
 * @author ADMIN
 */
public class UserDAO {
    private static Map<String, User> dictUsers = new ConcurrentHashMap<>();
    
    public static synchronized void AddDictUser(User value) {
        dictUsers.put(value.getUserID(), value);
        System.out.println("SIZE:" + dictUsers.size());
        
    }

    public static synchronized void RemoveDictUser(String userID) {
        dictUsers.remove(userID);
    }

    public static User GetUser(String userID) {
        return dictUsers.get(userID);
    }
    
    public static List<User> GetListUserCurrent() {
        List<User> tmp = new ArrayList<>();
        for(Map.Entry<String, User> entry : dictUsers.entrySet()) {
            tmp.add(entry.getValue());
        }
        return tmp;
    }
    
    public static synchronized boolean CheckUserLogined(String userID)  {
        return dictUsers.containsKey(userID);
    }
}
