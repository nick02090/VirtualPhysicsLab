export default {
    filters: {
        occupation(rawValue) {
            switch (rawValue) {
                case 0:
                    return "profesor";
                case 1:
                    return "student";
                default:
                    break;
            }
        }
    }
}