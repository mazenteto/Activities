
import{z} from'zod';
import { requiredString } from '../util/util';
export const regiserSchema=z.object({
    email:z.string().email(),
    displayName:requiredString('Display Name'),
    password:requiredString('Password'),
})
export type RegiserSchema=z.infer<typeof regiserSchema>;