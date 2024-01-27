using PitchPerfect.DTO;
using PitchPerfect.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace PitchPerfect.UI
{
    public class UIRoomsListPage : UIPage
    {
        [SerializeField] private UIRoom _roomPrefab;
        [SerializeField] private Transform _roomsContainer;
        [SerializeField] private Button _joinButton;

        private UIRoom[] _rooms;
        private int _selectedRoomID = -1;
        
        public override void Show()
        {
            base.Show();

            RoomDTO[] rooms = ServerManager.Instance.GetRooms();

            PopulateRoomsList(rooms);
            _joinButton.interactable = false;
        }

        public void PopulateRoomsList(RoomDTO[] rooms)
        {
            if (_rooms != null)
            {
                foreach (var room in _rooms)
                {
                    Destroy(room.gameObject);
                }
            }
            
            _rooms = new UIRoom[rooms.Length];

            for (int i = 0; i < rooms.Length; i++)
            {
                var uiRoom = Instantiate(_roomPrefab, _roomsContainer);
                uiRoom.Setup(rooms[i].Id, rooms[i].Name, this);
                _rooms[i] = uiRoom;
            }
        }

        public void SelectRoom(int roomToSelect)
        {
            if (_selectedRoomID == roomToSelect)
                return;
            
            foreach (var room in _rooms)
            {
                if (room.Id != _selectedRoomID)
                    continue;
                
                room.Deselect();
            }

            _selectedRoomID = roomToSelect;
            _joinButton.interactable = true;
        }

        public void OnJoin()
        {
            
        }
    }
}