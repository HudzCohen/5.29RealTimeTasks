import { useState, useEffect, useContext, createContext } from "react";
import axios from "axios";

const AuthContext = createContext();

const AuthContextComponent = ({ children }) => {
    const [user, setUser] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        const loadUser = async () => {
            const { data } = await axios.get('/api/account/getcurrentuser');
            setUser(data);
            setIsLoading(false);
        }
       
        loadUser();
    }, []);

    if (isLoading) {
        return <div className="container" style={{marginTop: 80, textAlign: "center"}}>Loading...</div>
    }

    return (
        <AuthContext.Provider value={{ user, setUser }}>
            {children}
        </AuthContext.Provider>
    )
}

const useAuth = () => useContext(AuthContext);

export { AuthContextComponent, useAuth };