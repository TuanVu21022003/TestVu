/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package observer;

import java.util.HashMap;
import java.util.Map;

import observer.action.*;

/**
 *
 * @author pc
 */
public class Observer {
    private static Map<String, ActionObserver> dictAction = new HashMap<>();

    public static void Init() {
        // Register observers
        dictAction.put("login", new LoginObserver());
        dictAction.put("logout", new LogoutObserver());
        dictAction.put("shoot", new ShootObserver());
        dictAction.put("create_room", new CreateRoomObserver());
        dictAction.put("get_list_room", new GetListRoomObserver());
        dictAction.put("join_room", new JoinRoomObserver());
        dictAction.put("quit_room", new QuitRoomObserver());
        dictAction.put("start_game", new StartGameObserver());
        dictAction.put("loading_progress", new LoadingProgressObserver());
        dictAction.put("change_bet", new ChangeBetObserver());
    }
    
    public static ActionObserver getAction(String action) {
        return dictAction.get(action);
    }
}
