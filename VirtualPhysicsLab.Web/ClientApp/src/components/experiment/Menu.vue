<template>
    <div id="menu">
        <aside class="menu">
            <transition mode="out-in">
                <div v-if="isMenu">
                    <p class="menu-label">
                        <a class="button icon-btn" @click="changeMenu">
                            <span class="icon">
                                <i class="fas fa-ellipsis-h"></i>
                            </span>
                        </a>
                    </p>
                    <div v-for="(group, index) in menus" :key="index" class="menu-group">
                        <p class="menu-label">{{group.name}}</p>
                        <ul class="menu-list">
                            <li v-for="(menu, index) in group.menus" :key="index">
                                <a
                                    @click="changeMenu({'groupName': group.name, 'menuName': menu.name})"
                                    :class="{'is-active': activeMenu.name == menu.name}"
                                >{{menu.name}}</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <p v-else>
                    <span>
                        <div class="is-flex">
                            <p class="menu-label">
                                <a class="button icon-btn" @click="changeMenu">
                                    <span class="icon">
                                        <i class="fas fa-ellipsis-v"></i>
                                    </span>
                                </a>
                            </p>
                            <p class="menu-label">{{activeMenu.name}}</p>
                        </div>
                        <keep-alive>
                            <component
                                :is="activeMenu.component"
                                @changeMenu="changeMenu"
                                @notification="notification"
                                @error="error"
                            ></component>
                        </keep-alive>
                    </span>
                </p>
            </transition>
        </aside>
    </div>
</template>

<script>
import NoviElement from "@/components/experiment/menus/NoviElement.vue";
import UrediElement from "@/components/experiment/menus/UrediElement.vue";
import FizikaElementa from "@/components/experiment/menus/FizikaElementa.vue";
import Statistika from "@/components/experiment/menus/Statistika.vue";

export default {
    name: "Menu",
    data() {
        return {
            isMenu: true,
            activeMenu: {
                name: "Novi element",
                component: NoviElement
            },
            menus: [
                {
                    name: "Općenito",
                    menus: [
                        {
                            name: "Novi element",
                            component: NoviElement
                        },
                        {
                            name: "Uredi element",
                            component: UrediElement
                        }
                    ]
                },
                {
                    name: "Praćenje promjena",
                    menus: [
                        {
                            name: "Fizika elementa",
                            component: FizikaElementa
                        },
                        {
                            name: "Statistika",
                            component: Statistika
                        }
                    ]
                }
            ]
        };
    },
    components: {
        NoviElement,
        UrediElement,
        FizikaElementa,
        Statistika
    },
    methods: {
        changeMenu(menuData) {
            this.isMenu = !this.isMenu;
            if (menuData.groupName && menuData.menuName) {
                var group = this.menus.find(x => x.name === menuData.groupName);
                this.activeMenu = group.menus.find(
                    x => x.name == menuData.menuName
                );
                this.isMenu = false;
            } else {
                this.isMenu = true;
            }
        },
        notification(msg) {
            this.$emit("notification", msg);
        },
        error(msg) {
            this.$emit("error", msg);
        }
    }
};
</script>

<style scoped>
.menu-group {
    padding-bottom: 10px;
}
aside {
    margin-left: 10px;
}
.v-leave {
    opacity: 1;
}
.v-leave-active {
    transition: opacity 0.25s;
}
.v-leave-to {
    opacity: 0;
}
.v-enter {
    opacity: 0;
}
.v-enter-active {
    transition: opacity 0.5s;
}
.v-enter-to {
    opacity: 1;
}
</style>
