export class UserModel {
    userId: number;
    lastName: string;
    firstName: string;
    email: string;

    constructor(data: any) {
        this.userId = data.userId;
        this.lastName = data.lastName;
        this.firstName = data.firstName
        this.email = data.email;
    }
}