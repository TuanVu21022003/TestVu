package oceanshootertournament_server.thread;

import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.RoomDAO;
import org.json.simple.JSONObject;

public class LoadingProgressThread extends Thread {

    public String userID;
    public String roomID;
    public float progress;
    
    public LoadingProgressThread(String userID, String roomID, float progress) {
        this.userID = userID;
        this.roomID = roomID;
        this.progress = progress;
    }

    @Override
    public void run() {
        var room = RoomDAO.GetRoom(roomID);
        room.UpdatePlayerProgress(userID, progress);
        room.getPlayers().forEach(user -> {
            JSONObject jsonResponseOther = new JSONObject();
            jsonResponseOther.put("idRole", userID);
            jsonResponseOther.put("action", "loading_progress");
            JSONObject jsonResponseProgress = new JSONObject();
            jsonResponseProgress.put("progress", progress);
            jsonResponseOther.put("data", jsonResponseProgress);
            String messOther = jsonResponseOther.toString();
            OceanShooterTournament_Server.SendResponse(messOther, user.getAddress(), user.getPort());
        });
    }
}
