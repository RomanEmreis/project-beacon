import { FC } from "react"
import ConnectionHub from "./features/beacon/hub/ConnectionHub"
import { useAppSelector } from "./app/hooks";
import { selectCurrentUserName } from "./features/beacon/RoomSlice";

import "./App.scss"
import { selectOnlineUsers } from "./features/beacon/ConnectionSlice";

const App: FC = () => {
    const userName = useAppSelector(selectCurrentUserName);
    const onlineUsers = useAppSelector(selectOnlineUsers);

    return (
        <div className="App">
            <header>
                <div>
                    <span>SignalR Audio Caller</span>
                    <div>
                        <span>
                            You are <span>{userName}</span>
                        </span>
                    </div>
                </div>
            </header>
            <ConnectionHub>
                <div>
                    <div>
                        <div>
                            <div>Idle</div>
                            <button>Hang Up</button>
                        </div>
                        <div>
                            <span>Online Users: <small>{onlineUsers.length}</small></span>
                            <ul>
                                {onlineUsers.map(({ name, inCall }) => (
                                    <li>
                                        <a href="#">
                                            <div>{name}</div>
                                            <div>{inCall ? 'Busy' : 'Available'}</div>
                                        </a>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </div>
                    <div>
                        <div>
                            <div>
                                <h4>Partner</h4>
                                <audio className="audio partner"></audio>
                            </div>
                        </div>
                    </div>
                </div>
            </ConnectionHub>
        </div>
    )
}

export default App
