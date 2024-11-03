/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.model.User;

import java.util.List;
import java.util.UUID;
import java.util.concurrent.CopyOnWriteArrayList;
import oceanshootertournament_server.dao.RoomDAO;
import oceanshootertournament_server.model.Room;
import org.json.simple.JSONObject;

/**
 *
 * @author DELL
 */
public class CreateRoomThread extends Thread {

    private User roomCreator;
    static final int MAX_PLAYERS = 4;

    public CreateRoomThread(User roomCreator) {
        this.roomCreator = roomCreator;
    }

    @Override
    public void run() {
        handleRoomCreation(this.roomCreator);
    }

    private void handleRoomCreation(User creator) {
        // Tạo ID phòng duy nhất sử dụng UUID
        Room room = new Room();
        room.setAdminID(creator.getUserID());
        // Thêm phòng vào danh sách phòng đang hoạt động và thông báo thành công
        RoomDAO.AddDictRoom(room);
        creator.setRoomCurrentID(room.getIdRoom());
        new JoinRoomThread(creator.getUserID(), room.getIdRoom()).start();
    }

    private String generateUniqueRoomID() {
        // Tạo ID phòng ngẫu nhiên sử dụng UUID
        return UUID.randomUUID().toString().substring(0, 6);  // Rút gọn xuống 6 ký tự
    }
}
