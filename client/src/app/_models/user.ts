export interface User{
    //A TS-interface is different from one in C#
    //The angular style guide suggests not to start it with an I... (i.e IUser)
    username: string;
    token: string;

}