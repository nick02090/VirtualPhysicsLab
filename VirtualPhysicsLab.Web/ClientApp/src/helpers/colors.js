export default {
    hexToColor3(hex) {
        var rgb = this.hexToRgb(hex);
        return this.rgbToColor3(rgb);
    },
    hexToRgb(hex) {
        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result ? {
            r: parseInt(result[1], 16),
            g: parseInt(result[2], 16),
            b: parseInt(result[3], 16)
        } : null;
    },
    rgbToColor3(rgb) {
        return [rgb.r / 255, rgb.g / 255, rgb.b / 255];
    }
}