using System.Collections.Generic;

public class AccountDataSDAO : ServerDataAccessObject<AccountData> { }

public class AllRoomDataSDAO : ServerDataAccessObject<List<JoinRoomData>> { }

public class RoomRequestDataSDAO : ServerDataAccessObject<JoinRoomData> { }

public class PlayerDataInRoomSDAO : ServerDataAccessObject<PlayerDataInRoom> { }

public class DataInRoomSDAO : ServerDataAccessObject<DataInRoomGame> { }

public class NewFrameGame : ServerDataAccessObject<PlatformData> { }