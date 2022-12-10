export class UserModel {
    userId: number;
    lastName: string;
    firstName: string;
    email: string;

    constructor();

    constructor(data: any);

    constructor(data?: any) {
        if (data ) {
            this.userId = data.userId;
            this.lastName = data.lastName;
            this.firstName = data.firstName
            this.email = data.email;
        } else {
            this.userId = 0;
            this.lastName = "";
            this.firstName = "";
            this.email = "";
        }
    }
}