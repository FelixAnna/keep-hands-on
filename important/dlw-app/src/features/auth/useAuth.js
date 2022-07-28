import { useContext, createContext } from 'react';

const authContext = createContext();

export function useAuth() {
  return useContext(authContext);
}

export default authContext;
