const BONEID_RIGHT_HAND = 57005;

const registeredObjects = {
    'phone': {
        objectName: 'p_amb_phone_01',
        boneId: BONEID_RIGHT_HAND,
        position: { x: 0.15, y: 0.0, z: -0.043, },
        rotation: { x: 15.0, y: 80.0, z: 150 },
        enterAnimation: { dict: 'cellphone@', name: 'cellphone_text_in', flag: 49, durationMs: 0 },
        exitAnimation: { dict: 'cellphone@', name: 'cellphone_text_out', flag: 49, durationMs: 0 }
    },
    'radio': {
        objectName: 'prop_cs_hand_radio',
        boneId: BONEID_RIGHT_HAND,
        position: { x: 0.15, y: 0.0, z: -0.043, },
        rotation: { x: 15.0, y: 80.0, z: 150 },
        enterAnimation: { dict: 'cellphone@', name: 'cellphone_text_in', flag: 49, durationMs: 0 },
        exitAnimation: { dict: 'cellphone@', name: 'cellphone_text_out', flag: 49, durationMs: 0 }
    },
    'tablet': {
        objectName: 'prop_cs_tablet',
        boneId: 60309,
        position: { x: 0.03, y: -0.000, z: 0, },
        rotation: { x: 10, y: 0, z: -10 },
        enterAnimation: { dict: 'amb@code_human_in_bus_passenger_idles@female@tablet@base', name: 'base', flag: 49, durationMs: 0 },
        exitAnimation: { dict: 'amb@code_human_in_bus_passenger_idles@female@tablet@base', name: 'base', flag: 49, durationMs: 0 }
    },
    'alcohol': {
        objectName: 'prop_cs_beer_bot_40oz',
        boneId: BONEID_RIGHT_HAND,
        position: { x: 0.13, y: 0.0, z: -0.05, },
        rotation: { x: 100, y: -220, z: 180 },
        enterAnimation: { dict: 'amb@world_human_drinking@beer@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 },
        exitAnimation: { dict: 'amb@world_human_drinking@beer@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 }
    },
    'drink': {
        objectName: 'prop_ld_flow_bottle',
        boneId: BONEID_RIGHT_HAND,
        position: { x: 0.12, y: 0.03, z: -0.04, },
        rotation: { x: 100, y: -220, z: 180 },
        enterAnimation: { dict: 'amb@world_human_drinking@beer@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 },
        exitAnimation: { dict: 'amb@world_human_drinking@beer@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 }
    },
    'burger': {
        objectName: 'prop_cs_burger_01',
        boneId: BONEID_RIGHT_HAND,
        position: { x: 0.15, y: -0.02, z: -0.05, },
        rotation: { x: -180, y: -150, z: -95 },
        enterAnimation: { dict: 'amb@code_human_wander_eating_donut@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 },
        exitAnimation: { dict: 'amb@code_human_wander_eating_donut@male@idle_a', name: 'idle_c', flag: 49, durationMs: 5000 }
    },
    'fishing-rod': {
        objectName: 'prop_fishing_rod_01',
        boneId: 60309,
        position: { x: 0, y: -0.01, z: 0.01, },
        rotation: { x: -180, y: -185, z: -175 },
        enterAnimation: { dict: 'amb@world_human_stand_fishing@idle_a', name: 'idle_c', flag: 15, durationMs: 0 },
        exitAnimation: { dict: 'amb@world_human_stand_fishing@idle_a', name: 'idle_c', flag: 15, durationMs: 0 }
    },
    'trash': {
        objectName: 'hei_prop_heist_binbag',
        boneId: 57005,
        position: { x: 0.12, y: 0, z: 0, },
        rotation: { x: 25, y: 270, z: 180 },
        enterAnimation: { dict: 'anim@heists@narcotics@trash', name: 'walk', flag: 49, durationMs: 0 },
        exitAnimation: { dict: 'anim@heists@narcotics@trash', name: 'throw_b', flag: 49, durationMs: 0 }
    },
    'clean': {
        objectName: 'prop_sponge_01',
        boneId: 28422,
        position: { x: 0.0, y: 0.0, z: -0.01, },
        rotation: { x: 90.0, y: 0.0, z: 0.0 },
        enterAnimation: { dict: 'timetable@floyd@clean_kitchen@base', name: 'base', flag: 49, durationMs: 5000 },
        exitAnimation: { dict: 'timetable@floyd@clean_kitchen@base', name: 'base', flag: 49, durationMs: 5000 }
    },
    'joint': {
        objectName: 'p_cs_joint_01',
        boneId: 57005,
        position: { x: 0.15, y: 0.02, z: 0, },
        rotation: { x: 0, y: 110, z: 0 },
        enterAnimation: { dict: 'amb@world_human_aa_smoke@male@idle_a', name: 'idle_c', flag: 49, durationMs: 180000 },
        exitAnimation: { dict: 'amb@world_human_aa_smoke@male@idle_a', name: 'idle_c', flag: 49, durationMs: 180000 }
    },
    'cigarette': {
        objectName: 'ng_proc_cigarette01a',
        boneId: 57005,
        position: { x: 0.15, y: 0.02, z: 0, },
        rotation: { x: 0, y: 110, z: 0 },
        enterAnimation: { dict: 'amb@world_human_aa_smoke@male@idle_a', name: 'idle_c', flag: 49, durationMs: 180000 },
        exitAnimation: { dict: 'amb@world_human_aa_smoke@male@idle_a', name: 'idle_c', flag: 49, durationMs: 180000 }
    },
    'pickaxe': {
        objectName: 'prop_tool_pickaxe',
        boneId: 57005,
        position: { x: 0.09, y: -0.18, z: -0.22, },
        rotation: { x: -55, y: -25, z: -30 },
        enterAnimation: { dict: 'melee@large_wpn@streamed_core', name: 'ground_attack_on_spot', flag: 49, durationMs: 180000 },
        exitAnimation: { dict: 'melee@large_wpn@streamed_core', name: 'ground_attack_on_spot', flag: 49, durationMs: 180000 }
    },
    'w_ar_carbineriflemk2': {
        objectName: 'w_ar_carbineriflemk2',
        boneId: 24816,
        position: { x: 0.12, y: 0.18, z: -0.15, },
        rotation: { x: 225, y: 175.0, z: 5 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    },
    'w_ar_carbinerifle': {
        objectName: 'w_ar_carbinerifle',
        boneId: 24816,
        position: { x: 0.12, y: 0.18, z: -0.15, },
        rotation: { x: 225, y: 175.0, z: 5 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    },
    'w_ar_assaultrifle': {
        objectName: 'w_ar_assaultrifle',
        boneId: 24816,
        position: { x: 0.25, y: -0.16, z: 0, },
        rotation: { x: 0, y: 165.0, z: 0 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    },
    'w_sb_smg': {
        objectName: 'w_sb_smg',
        boneId: 24816,
        position: { x: 0.12, y: 0.18, z: -0.15, },
        rotation: { x: 225, y: 175.0, z: 5 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    },
    'w_sb_gusenberg': {
        objectName: 'w_sb_gusenberg',
        boneId: 24816,
        position: { x: 0.25, y: -0.16, z: 0, },
        rotation: { x: 0, y: 165.0, z: 0 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    },
    'w_sg_pumpshotgun': {
        objectName: 'w_sg_pumpshotgun',
        boneId: 24816,
        position: { x: 0.12, y: 0.18, z: -0.15, },
        rotation: { x: 225, y: 175.0, z: 5 },
        enterAnimation: { dict: '', name: '', flag: 49, durationMs: 0 },
        exitAnimation: { dict: '', name: '', flag: 49, durationMs: 0 }
    }
};

export default registeredObjects;