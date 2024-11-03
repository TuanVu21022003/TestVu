/*
 * Click nbfs://nbhost/SystemFileSystem/Templates/Licenses/license-default.txt to change this license
 * Click nbfs://nbhost/SystemFileSystem/Templates/Classes/Class.java to edit this template
 */
package oceanshootertournament_server.thread;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;
import oceanshootertournament_server.OceanShooterTournament_Server;
import oceanshootertournament_server.dao.UserDAO;
import oceanshootertournament_server.model.User;
import org.json.simple.JSONObject;

/**
 *
 * @author pc
 */
public class LoginThread extends Thread {

    private User userLogin;

    public LoginThread(User userLogin) {
        this.userLogin = userLogin;
    }

    @Override
    public void run() {
        handleLogin(userLogin);
    }

    private void handleLogin(User user) {
        String account = user.getUsername();
        String password = user.getPassword();
        String filePath = "src/oceanshootertournament_server/data/users.txt";

        // kiểm tra đăng nhập
        if (checkLogin(account, password, filePath)) {
            if (!UserDAO.CheckUserLogined(user.getUserID())) {
                // gửi thông báo thành công
                UserDAO.AddDictUser(userLogin);
                JSONObject jsonResponse = new JSONObject();
                jsonResponse.put("idRole", "client");
                jsonResponse.put("action", "login_complete");
                jsonResponse.put("data", user.GetData());
                String mess = jsonResponse.toString();
                OceanShooterTournament_Server.SendResponse(mess, userLogin.getAddress(), userLogin.getPort());
                System.out.println("Response from server: " + mess);
            } else {
                // gửi thông báo thất bại
                JSONObject jsonResponse = new JSONObject();
                jsonResponse.put("idRole", "client");
                jsonResponse.put("action", "login_fail");
                jsonResponse.put("data", "Tai khoan da dang nhap o may khac");
                String mess = jsonResponse.toString();
                OceanShooterTournament_Server.SendResponse(mess, userLogin.getAddress(), userLogin.getPort());
            }
        } else {
            // gửi thông báo thất bại
            JSONObject jsonResponse = new JSONObject();
            jsonResponse.put("idRole", "client");
            jsonResponse.put("action", "login_fail");
            jsonResponse.put("data", "Sai tai khoan hoac mat khau");
            String mess = jsonResponse.toString();
            OceanShooterTournament_Server.SendResponse(mess, userLogin.getAddress(), userLogin.getPort());
        }
    }

    private static boolean checkLogin(String account, String password, String filePath) {
        try (BufferedReader br = new BufferedReader(new FileReader(filePath))) {
            String line;
            while ((line = br.readLine()) != null) {
                String[] credentials = line.split(":");
                if (credentials.length == 2) {
                    String storedAccount = credentials[0].trim();
                    String storedPassword = credentials[1].trim();
                    if (account.equals(storedAccount) && password.equals(storedPassword)) {
                        return true;
                    }
                }
            }
        } catch (IOException e) {
            System.err.println("Error reading user file: " + e.getMessage());
        }
        return false;
    }
}
