export class UserModel {
    userId: number;
    lastName: string;
    firstName: string;
    email: string;

    constructor(userId: number, lastName: string, firstName: string, email: string) {
        this.userId = userId;
        this.lastName = lastName
        this.firstName = firstName
        this.email = email;
    }
}